using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurLasalle.ApiClients;
using OurLasalle.Models;
using System.Security.Claims;

namespace OurLasalle.Pages.Courses
{
    [Authorize(Roles = "Teacher")]
    public class AddCourseModel : PageModel
    {
        private readonly CourseServiceClient _courseServiceClient;
        public AddCourseModel(CourseServiceClient courseServiceClient)
        {
            _courseServiceClient = courseServiceClient;
        }

        [BindProperty]
        public CreateCourseRequest CreateCourseRequest { get; set; } = new CreateCourseRequest();

        public void OnGet()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            CreateCourseRequest = new CreateCourseRequest
            {
                TeacherId = Guid.Parse(userId)
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _courseServiceClient.CreateCourseAsync(CreateCourseRequest);
            return RedirectToPage("/Courses/AllCourses");
        }
    }
}
