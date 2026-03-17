using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jobs_service_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeUrlToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResumeUrl",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeUrl",
                table: "Applications");
        }
    }
}
