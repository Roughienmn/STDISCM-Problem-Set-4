using Microsoft.EntityFrameworkCore;
using Grades.Entities;

namespace Grades.Data
{
    public class GradeDbContext(DbContextOptions<GradeDbContext> options) : DbContext(options)
    {
        public DbSet<Grade> Grades { get; set; }
        //public DbSet<Teacher> Teachers { get; set; }
    }
}
