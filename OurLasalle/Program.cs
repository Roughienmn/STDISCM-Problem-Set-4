using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OurLasalle.ApiClients;
using OurLasalle.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpClient<AuthServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("AuthApiBaseUrl"));
});
builder.Services.AddHttpClient<CourseServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("CourseApiBaseUrl"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = async context =>
            {
                var accessToken = context.Request.Cookies["AccessToken"];
                var refreshToken = context.Request.Cookies["RefreshToken"];

                if (!string.IsNullOrEmpty(accessToken))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

                    if (jwtToken != null && jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        // Token is expired, attempt to refresh it
                        if (!string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(jwtToken.Subject))
                        {
                            var authServiceClient = context.HttpContext.RequestServices.GetRequiredService<AuthServiceClient>();
                            var refreshTokenRequest = new RefreshTokenRequestDto
                            {
                                UserId = Guid.Parse(jwtToken.Subject),
                                RefreshToken = refreshToken
                            };

                            var tokenResponse = await authServiceClient.RefreshTokensAsync(refreshTokenRequest);
                            if (tokenResponse != null)
                            {
                                // Store the new tokens in cookies
                                context.Response.Cookies.Append("AccessToken", tokenResponse.AccessToken, new CookieOptions
                                {
                                    HttpOnly = true,
                                    Secure = true,
                                    SameSite = SameSiteMode.Strict
                                });

                                context.Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, new CookieOptions
                                {
                                    HttpOnly = true,
                                    Secure = true,
                                    SameSite = SameSiteMode.Strict
                                });

                                context.Token = tokenResponse.AccessToken;
                            }
                            else
                            {
                                // Refresh token is invalid, clear the cookies
                                context.Response.Cookies.Delete("AccessToken");
                                context.Response.Cookies.Delete("RefreshToken");
                            }
                        }
                        else
                        {
                            // No refresh token available or jwtToken.Subject is null, clear the cookies
                            context.Response.Cookies.Delete("AccessToken");
                            context.Response.Cookies.Delete("RefreshToken");
                        }
                    }
                    else
                    {
                        context.Token = accessToken;
                    }
                }

                await Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { message = "Authentication failed." });
                return context.Response.WriteAsync(result);
            }
        };

    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Redirect root URL based on authentication status
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        if (!context.Response.HasStarted)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/Home");
            }
            else
            {
                context.Response.Redirect("/Login");
            }
            return;
        }
    }
    if (!context.Response.HasStarted)
    {
        await next();
    }
});
app.MapRazorPages();

app.Run();
