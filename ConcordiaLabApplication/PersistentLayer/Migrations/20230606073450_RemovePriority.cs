using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistentLayer.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Priorities_PriorityId",
                table: "Experiments");

            migrationBuilder.DropTable(
                name: "Priorities");

            migrationBuilder.DropIndex(
                name: "IX_Experiments_PriorityId",
                table: "Experiments");

            migrationBuilder.DropColumn(
                name: "PriorityId",
                table: "Experiments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriorityId",
                table: "Experiments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Priorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrelloId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priorities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_PriorityId",
                table: "Experiments",
                column: "PriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_Priorities_PriorityId",
                table: "Experiments",
                column: "PriorityId",
                principalTable: "Priorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
