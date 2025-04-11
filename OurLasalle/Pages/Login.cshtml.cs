using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurLasalle.ApiClients;
using OurLasalle.Models;
using System.Threading.Tasks;

namespace OurLasalle.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthServiceClient _authServiceClient;

        public LoginModel(AuthServiceClient authServiceClient)
        {
            _authServiceClient = authServiceClient;
        }

        [BindProperty]
        public UserDto UserDto { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Request.Cookies.ContainsKey("AccessToken") && HttpContext.Request.Cookies.ContainsKey("RefreshToken"))
            {
                return RedirectToPage("/Home");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var tokenResponse = await _authServiceClient.LoginAsync(UserDto);
            if (tokenResponse == null)
            {
                ErrorMessage = "Invalid login attempt.";
                return Page();
            }

            // Store the token in an HTTP cookie
            HttpContext.Response.Cookies.Append("AccessToken", tokenResponse.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            HttpContext.Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            // Redirect to the home page after successful login
            return RedirectToPage("/Home");
        }
    }
}
