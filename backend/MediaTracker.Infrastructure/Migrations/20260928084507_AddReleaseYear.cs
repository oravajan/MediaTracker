using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReleaseYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReleaseYear",
                table: "Season",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReleaseYear",
                table: "Media",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseYear",
                table: "Season");

            migrationBuilder.DropColumn(
                name: "ReleaseYear",
                table: "Media");
        }
    }
}
