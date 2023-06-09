using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersistentLayer.Migrations
{
    /// <inheritdoc />
    public partial class ListData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EntityLists",
                columns: new[] { "Id", "Title", "TrelloId" },
                values: new object[,]
                {
                    { 1, "to do", "64760804e47275c707e05d38" },
                    { 2, "in progress", "64760804e47275c707e05d39" },
                    { 3, "completed", "64760804e47275c707e05d3a" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EntityLists",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EntityLists",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EntityLists",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
