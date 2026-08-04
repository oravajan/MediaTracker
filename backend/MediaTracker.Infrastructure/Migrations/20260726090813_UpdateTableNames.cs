using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Seasons_SeasonId",
                table: "Episodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Media_TvShowId",
                table: "Seasons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seasons",
                table: "Seasons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Episodes",
                table: "Episodes");

            migrationBuilder.RenameTable(
                name: "Seasons",
                newName: "Season");

            migrationBuilder.RenameTable(
                name: "Episodes",
                newName: "Episode");

            migrationBuilder.RenameIndex(
                name: "IX_Seasons_TvShowId",
                table: "Season",
                newName: "IX_Season_TvShowId");

            migrationBuilder.RenameIndex(
                name: "IX_Episodes_SeasonId",
                table: "Episode",
                newName: "IX_Episode_SeasonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Season",
                table: "Season",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Episode",
                table: "Episode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Episode_Season_SeasonId",
                table: "Episode",
                column: "SeasonId",
                principalTable: "Season",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Season_Media_TvShowId",
                table: "Season",
                column: "TvShowId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episode_Season_SeasonId",
                table: "Episode");

            migrationBuilder.DropForeignKey(
                name: "FK_Season_Media_TvShowId",
                table: "Season");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Season",
                table: "Season");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Episode",
                table: "Episode");

            migrationBuilder.RenameTable(
                name: "Season",
                newName: "Seasons");

            migrationBuilder.RenameTable(
                name: "Episode",
                newName: "Episodes");

            migrationBuilder.RenameIndex(
                name: "IX_Season_TvShowId",
                table: "Seasons",
                newName: "IX_Seasons_TvShowId");

            migrationBuilder.RenameIndex(
                name: "IX_Episode_SeasonId",
                table: "Episodes",
                newName: "IX_Episodes_SeasonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seasons",
                table: "Seasons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Episodes",
                table: "Episodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Seasons_SeasonId",
                table: "Episodes",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Media_TvShowId",
                table: "Seasons",
                column: "TvShowId",
                principalTable: "Media",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
