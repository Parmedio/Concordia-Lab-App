using Microsoft.EntityFrameworkCore.Migrations;


#nullable disable

namespace PersistentLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_EntityLists_ListId",
                table: "Experiments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntityLists",
                table: "EntityLists");

            migrationBuilder.RenameTable(
                name: "EntityLists",
                newName: "Column");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Column",
                table: "Column",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_Column_ListId",
                table: "Experiments",
                column: "ListId",
                principalTable: "Column",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Column_ListId",
                table: "Experiments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Column",
                table: "Column");

            migrationBuilder.RenameTable(
                name: "Column",
                newName: "EntityLists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntityLists",
                table: "EntityLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_EntityLists_ListId",
                table: "Experiments",
                column: "ListId",
                principalTable: "EntityLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
