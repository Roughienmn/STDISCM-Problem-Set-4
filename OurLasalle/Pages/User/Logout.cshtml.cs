using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OurLasalle.Pages.User
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            // Remove the authentication cookies
            HttpContext.Response.Cookies.Delete("AccessToken");
            HttpContext.Response.Cookies.Delete("RefreshToken");

            // Redirect to the login page after logout
            return RedirectToPage("/Login");
        }
    }
}
