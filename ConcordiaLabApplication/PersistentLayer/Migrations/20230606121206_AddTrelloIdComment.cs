using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistentLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddTrelloIdComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrelloId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrelloId",
                table: "Comments");
        }
    }
}
