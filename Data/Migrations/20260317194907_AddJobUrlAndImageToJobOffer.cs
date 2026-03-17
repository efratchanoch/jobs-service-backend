using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jobs_service_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddJobUrlAndImageToJobOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobWebsiteUrl",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobWebsiteUrl",
                table: "Jobs");
        }
    }
}
