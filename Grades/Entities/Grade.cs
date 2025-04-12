namespace Grades.Entities
{
    public class Grade
    {
        public Guid Id { get; set;  }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public int GPA { get; set; }
        public string CourseName { get; set; } = string.Empty;
    }
}

