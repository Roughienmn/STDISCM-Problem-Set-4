using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OurLasalle.ApiClients;
using OurLasalle.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurLasalle.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AuthServiceClient _authServiceClient;

        public RegisterModel(AuthServiceClient authServiceClient)
        {
            _authServiceClient = authServiceClient;
        }

        [BindProperty]
        public UserDto UserDto { get; set; } = new UserDto();

        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Student", Text = "Student" },
            new SelectListItem { Value = "Teacher", Text = "Teacher" }
        };

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Set the default role to "Student"
            UserDto.Role = "Student";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _authServiceClient.RegisterAsync(UserDto);
            if (user == null)
            {
                ErrorMessage = "Username already exists.";
                return Page();
            }

            // Handle successful registration (e.g., redirect to login page)
            return RedirectToPage("/Login");
        }
    }
}
