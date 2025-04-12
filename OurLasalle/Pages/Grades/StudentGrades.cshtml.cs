using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurLasalle.ApiClients;
using OurLasalle.Models;
using System.Security.Claims;

namespace OurLasalle.Pages.Grades
{
    public class StudentGradesModel : PageModel // Renamed class
    {
        private readonly GradeServiceClient _gradeServiceClient;
        private readonly CourseServiceClient _courseServiceClient;
        public StudentGradesModel(GradeServiceClient gradeServiceClient, CourseServiceClient courseServiceClient)
        {
            _gradeServiceClient = gradeServiceClient;
            _courseServiceClient = courseServiceClient;
        }

        public List<GradeDto> Grades { get; private set; }
        public List<CourseDto> Courses { get; private set; }
        public int GPA { get; set; }

        public async Task<IActionResult> OnGetGradesByCourseIdAsync(Guid courseId)
        {
            Grades = await _gradeServiceClient.GetGradesByCourseIdAsync(courseId);

            if (Grades == null || !Grades.Any())
            {
                ModelState.AddModelError(string.Empty, "No grades found for the specified course.");
                return Page();
            }

            return Page();
        }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return;
            }

            // Fetch grades for the student
            Grades = await _gradeServiceClient.GetGradesByStudentIdAsync(Guid.Parse(userId));

            // Fetch all courses
            Courses = await _courseServiceClient.GetAllCoursesAsync();

            // Enrich grades with CourseName
            foreach (var grade in Grades)
            {
                grade.CourseName = Courses.FirstOrDefault(c => c.Id == grade.CourseId)?.Name ?? "Unknown";
            }
        }

    }
}
