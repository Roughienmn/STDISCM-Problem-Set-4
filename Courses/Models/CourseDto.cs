namespace Courses.Models
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public Guid TeacherId { get; set; }
        public List<StudentDto>? Students { get; set; }
    }

}
