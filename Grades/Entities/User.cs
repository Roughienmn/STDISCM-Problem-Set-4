namespace Grades.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<Grade> Grades { get; set; } = new();
    }
}
