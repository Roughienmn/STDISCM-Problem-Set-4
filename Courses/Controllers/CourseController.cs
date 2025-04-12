using Courses.Entities;
using Courses.Models;
using Courses.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Courses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<CourseDto>> CreateCourse(CreateCourseRequest request)
        {
            var course = await courseService.CreateCourseAsync(request);
            if (course is null)
            {
                return BadRequest("Course already exists.");
            }
            return Ok(course);
        }

        [HttpPost("enroll")]
        public async Task<ActionResult<EnrollRequestDto>> EnrollStudent(EnrollRequestDto request)
        {
            var result = await courseService.EnrollStudentAsync(request);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("id")]
        public async Task<ActionResult<CourseDto>> GetCourse(Guid id)
        {
            var course = await courseService.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult<List<CourseDto>>> GetCoursesByTeacherId(Guid teacherId)
        {
            var courses = await courseService.GetCoursesByTeacherIdAsync(teacherId);
            if (courses == null || !courses.Any())
            {
                return NotFound("No courses found for the specified teacher.");
            }
            return Ok(courses);
        }

        [HttpGet("course/{courseId}/students")]
        public async Task<ActionResult<List<StudentDto>>> GetStudentsByCourseId(Guid courseId)
        {
            var course = await courseService.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                return NotFound("Course not found.");
            }
            return Ok(course.Students);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<CourseDto>>> GetAllCourses()
        {
            var courses = await courseService.GetAllCoursesAsync();
            return Ok(courses);
        }
    }
}