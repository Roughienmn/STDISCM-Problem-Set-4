using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grades.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseNameToGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Grades",
                newName: "CourseName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseName",
                table: "Grades",
                newName: "Name");
        }
    }
}
