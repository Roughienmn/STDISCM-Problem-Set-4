using Grades.Entities;
using Grades.Models;
using Grades.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grades.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService gradeService;
        public GradeController(IGradeService gradeService)
        {
            this.gradeService = gradeService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<GradeDto>> CreateGrade(CreateGradeRequest request)
        {
            var grade = await gradeService.CreateGradeAsync(request);
            if (grade is null)
            {
                return BadRequest("Grade already exists.");
            }
            return Ok(grade);
        }

        [HttpPost("addgrade")]
        public async Task<IActionResult> AddGrade(AddGradeRequestDto request)
        {
            // Find the specific grade for the given StudentId and CourseId
            var existingGrade = (await gradeService.GetGradesByCourseIdAsync(request.CourseId))
                .FirstOrDefault(g => g.StudentId == request.StudentId);

            if (existingGrade != null)
            {
                // Update the existing grade
                existingGrade.GPA = request.GPA;
                var updated = await gradeService.AddGradeAsync(new AddGradeRequestDto
                {
                    StudentId = existingGrade.StudentId,
                    CourseId = existingGrade.CourseId,
                    GPA = existingGrade.GPA
                });
                if (!updated)
                {
                    return BadRequest("Failed to update the grade.");
                }
                return Ok("Grade updated successfully.");
            }

            // Add a new grade
            var result = await gradeService.AddGradeAsync(request);
            if (!result)
            {
                return BadRequest("Failed to add the grade.");
            }

            return Ok("Grade added successfully.");
        }

        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<List<GradeDto>>> GetGradesByCourseId(Guid courseId)
        {
            var grades = await gradeService.GetGradesByCourseIdAsync(courseId);
            if (grades == null || !grades.Any())
            {
                return NotFound("No grades found for the specified course.");
            }
            return Ok(grades);
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<GradeDto>>> GetGradesByStudentId(Guid studentId)
        {
            var grades = await gradeService.GetGradesByStudentIdAsync(studentId);
            if (grades == null || !grades.Any())
            {
                return NotFound("No grades found for the specified student.");
            }
            return Ok(grades);
        }



        [HttpGet("all")]
        public async Task<ActionResult<List<GradeDto>>> GetAllGrades()
        {
            var grades = await gradeService.GetAllGradesAsync();
            return Ok(grades);
        }
    }
}
