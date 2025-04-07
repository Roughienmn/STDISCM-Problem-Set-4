using Microsoft.EntityFrameworkCore;
using Courses.Entities;

namespace Courses.Data
{
    public class CourseDbContext(DbContextOptions<CourseDbContext> options) : DbContext(options)
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
