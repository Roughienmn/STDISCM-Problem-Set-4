using Grades.Entities;

namespace Grades.Models
{
    public class CreateGradeRequest
    {
        public string CourseName { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public Guid TeacherId{ get; set; }
        public int GPA { get; set; }
    }
}
