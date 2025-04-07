namespace Courses.Entities
{
    public class Course
    {
        public Guid Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public Guid TeacherId { get; set; }
        public List<Student>? Students { get; set; } = new();
    }
}
