namespace OurLasalle.Models
{
    public class CreateGradeRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public Guid TeacherId{ get; set; }
        public int GPA { get; set; }
    }
}
