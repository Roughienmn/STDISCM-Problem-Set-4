using Courses.Entities;
using Courses.Models;

namespace Courses.Services
{
    public interface ICourseService
    {
        Task<Course?> CreateCourseAsync(CreateCourseRequest request);
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(Guid id);
        Task<bool> EnrollStudentAsync(EnrollRequestDto request);
    }
}