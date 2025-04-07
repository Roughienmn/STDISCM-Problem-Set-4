using Courses.Data;
using Courses.Entities;
using Courses.Models;
using Microsoft.EntityFrameworkCore;

namespace Courses.Services
{
    public class CourseService : ICourseService
    {
        private readonly CourseDbContext context;

        public CourseService(CourseDbContext context)
        {
            this.context = context;
        }

        public async Task<Course?> CreateCourseAsync(CreateCourseRequest request)
        {
            if (await context.Courses.AnyAsync(c => c.Name == request.Name))
            {
                return null;
            }

            var course = new Course
            {
                Name = request.Name,
                Capacity = request.Capacity,
                TeacherId = request.TeacherId,
                Students = new List<Student>()
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            return course;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            return await context.Courses
                .Include(c => c.Students)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Capacity = c.Capacity,
                    TeacherId = c.TeacherId,
                    Students = c.Students.Select(s => new StudentDto
                    {
                        Id = s.Id
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<CourseDto?> GetCourseByIdAsync(Guid id)
        {
            var course = await context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return null;
            }

            return new CourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Capacity = course.Capacity,
                TeacherId = course.TeacherId,
                Students = course.Students.Select(s => new StudentDto
                {
                    Id = s.Id
                }).ToList()
            };
        }

        public async Task<bool> EnrollStudentAsync(EnrollRequestDto request)
        {
            var courseId = request.CourseId;
            var studentId = request.StudentId;
            var course = await context.Courses.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == courseId);
            if (course is null)
            {
                return false;
            }
            var student = await context.Students.FindAsync(studentId);
            if (student is null)
            {
                student = new Student
                {
                    Id = studentId
                };
                context.Students.Add(student);
            }
            if (course.Students == null)
            {
                course.Students = new List<Student>();
            }
            if (course.Students.Count >= course.Capacity)
            {
                return false;
            }
            course.Students.Add(student);
            await context.SaveChangesAsync();
            return true;
        }
    }
}