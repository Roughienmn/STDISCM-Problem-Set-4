using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurLasalle.ApiClients;
using OurLasalle.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    private readonly AuthServiceClient _authServiceClient;

    public IndexModel(AuthServiceClient authServiceClient)
    {
        _authServiceClient = authServiceClient;
    }

    [BindProperty]
    public UserDto UserDto { get; set; }

    public string ErrorMessage { get; set; }

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
        else

            // Handle successful login (e.g., set cookies, redirect, etc.)
            // For example:
            // HttpContext.Response.Cookies.Append("AccessToken", tokenResponse.AccessToken);

            return RedirectToPage("/Login");
    }
}
