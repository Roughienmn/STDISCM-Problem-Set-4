using Grades.Data;
using Grades.Entities;
using Grades.Models;
using Microsoft.EntityFrameworkCore;

namespace Grades.Services
{
    public class GradeService : IGradeService
    {
        private readonly GradeDbContext context;
        public GradeService(GradeDbContext context)
        {
            this.context = context;
        }

        public async Task<GradeDto?> CreateGradeAsync(CreateGradeRequest request)
        {
            if (await context.Grades.AnyAsync(g => g.CourseName == request.CourseName))
            {
                return null;
            }
            var grade = new Grade
            {
                CourseName = request.CourseName,
                //StudentId = request.StudentId,
                CourseId = request.CourseId,
                //TeacherId = request.TeacherId,
                GPA = request.GPA
            };
            context.Grades.Add(grade);
            await context.SaveChangesAsync();
            return new GradeDto
            {
                Id = grade.Id,
                Name = request.CourseName,
                //StudentId = request.StudentId,
                CourseId = request.CourseId,
                //TeacherId = request.TeacherId,
                GPA = request.GPA
            };
        }
        public async Task<List<GradeDto>> GetAllGradesAsync()
        {
            return await context.Grades
                .Select(g => new GradeDto
                {
                    Id = g.Id,
                    StudentId = g.StudentId,
                    CourseId = g.CourseId,
                    TeacherId = g.TeacherId,
                    GPA = g.GPA,
                    Name = g.CourseName
                }).ToListAsync();
        }

        public async Task<List<GradeDto>> GetGradesByCourseIdAsync(Guid courseId)
        {
            var grades = await context.Grades
                .Where(g => g.CourseId == courseId)
                .Select(g => new GradeDto
                {
                    Id = g.Id,
                    Name = g.CourseName,
                    StudentId = g.StudentId,
                    CourseId = g.CourseId,
                    TeacherId = g.TeacherId,
                    GPA = g.GPA
                })
                .ToListAsync();

            return grades;
        }

        public async Task<List<GradeDto>> GetGradesByStudentIdAsync(Guid studentId)
        {
            return await context.Grades
                .Where(g => g.StudentId == studentId)
                .Select(g => new GradeDto
                {
                    Id = g.Id,
                    Name = g.CourseName,
                    StudentId = g.StudentId,
                    CourseId = g.CourseId,
                    TeacherId = g.TeacherId,
                    GPA = g.GPA
                })
                .ToListAsync();
        }
        public async Task<bool> AddGradeAsync(AddGradeRequestDto request)
        {
            // Check if the grade already exists for the given StudentId and CourseId
            var existingGrade = await context.Grades
                .FirstOrDefaultAsync(g => g.StudentId == request.StudentId && g.CourseId == request.CourseId);

            if (existingGrade != null)
            {
                // Update the existing grade
                existingGrade.GPA = request.GPA;
                existingGrade.TeacherId = request.TeacherId;
                existingGrade.CourseName = request.CourseName;
                context.Grades.Update(existingGrade);
            }
            else
            {
                // Add a new grade
                var newGrade = new Grade
                {
                    StudentId = request.StudentId,
                    CourseId = request.CourseId,
                    TeacherId = request.TeacherId, // Save TeacherId
                    GPA = request.GPA,
                    CourseName = request.CourseName
                };
                context.Grades.Add(newGrade);
            }

            await context.SaveChangesAsync();
            return true;
        }
    }
}
