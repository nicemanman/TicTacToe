using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeveralFields2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "IsOpponentTurn",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "OpponentId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "OpponentIsBot",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    UUID = table.Column<Guid>(type: "uuid", nullable: false),
                    Player1Id = table.Column<string>(type: "text", nullable: false),
                    Player2Id = table.Column<string>(type: "text", nullable: false),
                    PlayerIdTurn = table.Column<string>(type: "text", nullable: false),
                    PlayerIdWin = table.Column<string>(type: "text", nullable: false),
                    JoinCode = table.Column<string>(type: "text", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GameUUID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.UUID);
                    table.ForeignKey(
                        name: "FK_GameSessions_Games_GameUUID",
                        column: x => x.GameUUID,
                        principalTable: "Games",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_GameUUID",
                table: "GameSessions",
                column: "GameUUID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.AddColumn<long>(
                name: "CreatorId",
                table: "Games",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpponentTurn",
                table: "Games",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "OpponentId",
                table: "Games",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "OpponentIsBot",
                table: "Games",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
