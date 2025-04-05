using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueIndexOnPlayerIdWithActiveGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameState",
                table: "GameSessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_Player1Id",
                table: "GameSessions",
                column: "Player1Id",
                unique: true,
                filter: "'GameState'='InProgress'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GameSessions_Player1Id",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "GameState",
                table: "GameSessions");
        }
    }
}
