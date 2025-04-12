namespace Grades.Models
{
    public class GradeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public int GPA { get; set; }
    }
}
