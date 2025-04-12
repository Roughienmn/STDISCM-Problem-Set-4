using Grades.Entities;
using Grades.Models;

namespace Grades.Services
{
    public interface IGradeService
    {
        Task<GradeDto?> CreateGradeAsync(CreateGradeRequest request);
        Task<List<GradeDto>> GetAllGradesAsync();
        Task<List<GradeDto?>> GetGradesByCourseIdAsync(Guid courseId);
        Task<List<GradeDto>> GetGradesByStudentIdAsync(Guid studentId);
        Task<bool> AddGradeAsync(AddGradeRequestDto request);
    }
}
