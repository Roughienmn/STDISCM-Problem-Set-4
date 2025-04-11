using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurLasalle.ApiClients;
using OurLasalle.Models;
using System.Security.Claims;

namespace OurLasalle.Pages.Courses
{
    public class AllCoursesModel : PageModel
    {
        private readonly CourseServiceClient _courseServiceClient;

        public AllCoursesModel(CourseServiceClient courseServiceClient)
        {
            _courseServiceClient = courseServiceClient;
        }

        public List<CourseDto> Courses { get; private set; }

        public async Task<IActionResult> OnPostEnrollAsync(Guid courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var enrollRequest = new EnrollRequestDto
            {
                CourseId = courseId,
                StudentId = Guid.Parse(userId)
            };

            await _courseServiceClient.EnrollStudentAsync(enrollRequest);

            // Reload the courses to reflect the updated enrollment
            Courses = await _courseServiceClient.GetAllCoursesAsync();

            return Page();
        }

        public async Task OnGetAsync()
        {
            Courses = await _courseServiceClient.GetAllCoursesAsync();
        }
    }
}
