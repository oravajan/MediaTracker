using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Media",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "WatchedAt",
                table: "Media",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WatchedAt",
                table: "Episode",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "WatchedAt",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "WatchedAt",
                table: "Episode");
        }
    }
}
