namespace OurLasalle.Models
{
    public class CreateCourseRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public Guid TeacherId { get; set; }
    }
}
