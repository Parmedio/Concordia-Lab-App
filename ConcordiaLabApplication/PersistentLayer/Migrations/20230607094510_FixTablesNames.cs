using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistentLayer.Migrations
{
    /// <inheritdoc />
    public partial class FixTablesNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Scientist_ScientistId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Lists_ListId",
                table: "Experiments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentScientist_Scientist_ScientistsId",
                table: "ExperimentScientist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Scientist",
                table: "Scientist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lists",
                table: "Lists");

            migrationBuilder.RenameTable(
                name: "Scientist",
                newName: "Scientists");

            migrationBuilder.RenameTable(
                name: "Lists",
                newName: "EntityLists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Scientists",
                table: "Scientists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntityLists",
                table: "EntityLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Scientists_ScientistId",
                table: "Comments",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_EntityLists_ListId",
                table: "Experiments",
                column: "ListId",
                principalTable: "EntityLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentScientist_Scientists_ScientistsId",
                table: "ExperimentScientist",
                column: "ScientistsId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Scientists_ScientistId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_EntityLists_ListId",
                table: "Experiments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentScientist_Scientists_ScientistsId",
                table: "ExperimentScientist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Scientists",
                table: "Scientists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntityLists",
                table: "EntityLists");

            migrationBuilder.RenameTable(
                name: "Scientists",
                newName: "Scientist");

            migrationBuilder.RenameTable(
                name: "EntityLists",
                newName: "Lists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Scientist",
                table: "Scientist",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lists",
                table: "Lists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Scientist_ScientistId",
                table: "Comments",
                column: "ScientistId",
                principalTable: "Scientist",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_Lists_ListId",
                table: "Experiments",
                column: "ListId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentScientist_Scientist_ScientistsId",
                table: "ExperimentScientist",
                column: "ScientistsId",
                principalTable: "Scientist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
