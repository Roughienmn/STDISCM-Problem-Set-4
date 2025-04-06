using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; }
    [BindProperty]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = new HttpClient();
        var loginPayload = new
        {
            username = Username,
            password = Password
        };

        var json = JsonSerializer.Serialize(loginPayload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("http://localhost:5000/api/auth/login", content); // Replace with your actual Auth API URL

        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = "Invalid username or password.";
            return Page();
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResult = JsonSerializer.Deserialize<TokenResponseDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // You can now store the token in a cookie or session
        HttpContext.Session.SetString("AccessToken", tokenResult.AccessToken);
        HttpContext.Session.SetString("RefreshToken", tokenResult.RefreshToken);

        return RedirectToPage("/Index");
    }

    public class TokenResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
