using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedstatefieldingamemodelandchangedchartypetostringtypeinGameMapFieldmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsX",
                table: "GameMapField");

            migrationBuilder.RenameColumn(
                name: "IndexY",
                table: "GameMapField",
                newName: "Row");

            migrationBuilder.RenameColumn(
                name: "IndexX",
                table: "GameMapField",
                newName: "Column");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Char",
                table: "GameMapField",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Char",
                table: "GameMapField");

            migrationBuilder.RenameColumn(
                name: "Row",
                table: "GameMapField",
                newName: "IndexY");

            migrationBuilder.RenameColumn(
                name: "Column",
                table: "GameMapField",
                newName: "IndexX");

            migrationBuilder.AddColumn<bool>(
                name: "IsX",
                table: "GameMapField",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
