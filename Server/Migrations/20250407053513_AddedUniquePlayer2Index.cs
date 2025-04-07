using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniquePlayer2Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_Player2Id",
                table: "GameSessions",
                column: "Player2Id",
                unique: true,
                filter: "'GameState'='InProgress'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GameSessions_Player2Id",
                table: "GameSessions");
        }
    }
}
