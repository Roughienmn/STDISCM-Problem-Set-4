namespace Grades.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public List<Grade> Grades { get; set; } = new();
    }
}
