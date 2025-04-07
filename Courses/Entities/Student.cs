namespace Courses.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public List<Course> Courses { get; set; } = new();
    }
}
