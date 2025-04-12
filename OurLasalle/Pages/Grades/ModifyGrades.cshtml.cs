using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OurLasalle.ApiClients;
using OurLasalle.Models;
using System.Security.Claims;

namespace OurLasalle.Pages.Grades
{
    public class ModifyGradesModel : PageModel
    {
        private readonly GradeServiceClient _gradeServiceClient;
        private readonly CourseServiceClient _courseServiceClient;

        public ModifyGradesModel(GradeServiceClient gradeServiceClient, CourseServiceClient courseServiceClient)
        {
            _gradeServiceClient = gradeServiceClient;
            _courseServiceClient = courseServiceClient;
        }

        [BindProperty(SupportsGet = true)]
        public Guid CourseId { get; set; }

        [BindProperty]
        public List<GradeDto> Grades { get; set; } = new();

        public string CourseName { get; set; } = string.Empty;

        public bool isTeacherOfCourse;

        public async Task OnGetAsync()
        {
            if (CourseId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "Invalid course ID.");
                return;
            }
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var coursesOfTeacher = await _courseServiceClient.GetCoursesByTeacherIdAsync(Guid.Parse(teacherId));
            var course = coursesOfTeacher.FirstOrDefault(c => c.Id == CourseId);

            if (course == null)
            {
                isTeacherOfCourse = false;
            }
            else
            {
                isTeacherOfCourse = true;
            }

                // Fetch students for the course
                var students = await _courseServiceClient.GetStudentsByCourseIdAsync(CourseId);

            // Fetch grades for the course
            Grades = await _gradeServiceClient.GetGradesByCourseIdAsync(CourseId);

            // Fetch all courses
            var courses = await _courseServiceClient.GetAllCoursesAsync();
            CourseName = courses.FirstOrDefault(c => c.Id == CourseId)?.Name ?? "Unknown";

            // Ensure all students have a grade entry and enrich with CourseName
            foreach (var student in students)
            {
                var grade = Grades.FirstOrDefault(g => g.StudentId == student.Id);
                if (grade == null)
                {
                    Grades.Add(new GradeDto
                    {
                        StudentId = student.Id,
                        CourseId = CourseId,
                        GPA = 0, // Default GPA
                        CourseName = CourseName
                    });
                }
                else
                {
                    grade.CourseName = CourseName;
                }
            }
        }

        public async Task<IActionResult> OnPostAsync([FromForm] Dictionary<Guid, int> grades)
        {
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(teacherId))
            {
                return Unauthorized();
            }

            // Ensure the logged-in user is the teacher for the course
            var courses = await _courseServiceClient.GetCoursesByTeacherIdAsync(Guid.Parse(teacherId));
            var course = courses.FirstOrDefault(c => c.Id == CourseId);
            if (courses == null)
            {
                return Forbid(); // User is not authorized to modify grades for this course
            }

            if (grades == null || !grades.Any())
            {
                ModelState.AddModelError(string.Empty, "Grades data is missing or invalid.");
                return Page();
            }

            // Update grades in bulk
            foreach (var grade in grades)
            {
                var addGradeRequest = new AddGradeRequestDto
                {
                    StudentId = grade.Key, 
                    CourseId = CourseId,
                    GPA = grade.Value, 
                    TeacherId = Guid.Parse(teacherId),
                    CourseName = course.Name
                };

                var success = await _gradeServiceClient.AddGradeAsync(addGradeRequest);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, $"Failed to update grade for student {grade.Key}.");
                }
            }

            // Reload the grades to reflect the updated data
            Grades = await _gradeServiceClient.GetGradesByCourseIdAsync(CourseId);
            CourseName = course.Name;
            TempData["SuccessMessage"] = "Changes have been successfully saved.";

            return Page();
        }
    }
}
