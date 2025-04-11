namespace Courses.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<Course> Courses { get; set; } = new();
    }
}
