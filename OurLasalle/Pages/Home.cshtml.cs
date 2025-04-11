using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace OurLasalle.Pages
{
    [Authorize(Roles = "Student,Teacher")]
    public class HomeModel : PageModel
    {
        private readonly ILogger<HomeModel> _logger;

        public HomeModel(ILogger<HomeModel> logger)
        {
            _logger = logger;
        }

        public string Username { get; set; }

        public void OnGet()
        {
            Username = User.FindFirst(ClaimTypes.Name)?.Value;

            // Log the claims for debugging
            foreach (var claim in User.Claims)
            {
                _logger.LogInformation($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
        }
    }
}
