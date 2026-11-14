using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueSeasonAndEpisodeNumbers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Season_TvShowId",
                table: "Season");

            migrationBuilder.DropIndex(
                name: "IX_Episode_SeasonId",
                table: "Episode");

            migrationBuilder.CreateIndex(
                name: "IX_Season_TvShowId_SeasonNumber",
                table: "Season",
                columns: new[] { "TvShowId", "SeasonNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Episode_SeasonId_EpisodeNumber",
                table: "Episode",
                columns: new[] { "SeasonId", "EpisodeNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Season_TvShowId_SeasonNumber",
                table: "Season");

            migrationBuilder.DropIndex(
                name: "IX_Episode_SeasonId_EpisodeNumber",
                table: "Episode");

            migrationBuilder.CreateIndex(
                name: "IX_Season_TvShowId",
                table: "Season",
                column: "TvShowId");

            migrationBuilder.CreateIndex(
                name: "IX_Episode_SeasonId",
                table: "Episode",
                column: "SeasonId");
        }
    }
}
