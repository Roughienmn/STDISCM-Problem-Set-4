namespace OurLasalle.Models
{
    public class GradeDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public int GPA { get; set; }
        public string CourseName { get; set; } = string.Empty;
    }
}
