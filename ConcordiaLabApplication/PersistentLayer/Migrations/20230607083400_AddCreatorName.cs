using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistentLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TrelloId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ScientistId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ScientistId",
                table: "Comments",
                column: "ScientistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Scientist_ScientistId",
                table: "Comments",
                column: "ScientistId",
                principalTable: "Scientist",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Scientist_ScientistId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ScientistId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ScientistId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "TrelloId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
