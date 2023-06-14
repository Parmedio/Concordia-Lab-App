using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersistentLayer.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrelloId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Labels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrelloId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scientists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrelloToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrelloMemberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scientists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Experiments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrelloId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeadLine = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LabelId = table.Column<int>(type: "int", nullable: true),
                    ListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiments_EntityLists_ListId",
                        column: x => x.ListId,
                        principalTable: "EntityLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiments_Labels_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Labels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrelloId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperimentId = table.Column<int>(type: "int", nullable: false),
                    ScientistId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Scientists_ScientistId",
                        column: x => x.ScientistId,
                        principalTable: "Scientists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExperimentScientist",
                columns: table => new
                {
                    ExperimentsId = table.Column<int>(type: "int", nullable: false),
                    ScientistsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentScientist", x => new { x.ExperimentsId, x.ScientistsId });
                    table.ForeignKey(
                        name: "FK_ExperimentScientist_Experiments_ExperimentsId",
                        column: x => x.ExperimentsId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentScientist_Scientists_ScientistsId",
                        column: x => x.ScientistsId,
                        principalTable: "Scientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EntityLists",
                columns: new[] { "Id", "Title", "TrelloId" },
                values: new object[,]
                {
                    { 1, "to do", "64760804e47275c707e05d38" },
                    { 2, "in progress", "64760804e47275c707e05d39" },
                    { 3, "completed", "64760804e47275c707e05d3a" }
                });

            migrationBuilder.InsertData(
                table: "Labels",
                columns: new[] { "Id", "Title", "TrelloId" },
                values: new object[,]
                {
                    { 1, "Medium", "647609751afdaf2b05536cd9" },
                    { 2, "Low", "647609751afdaf2b05536cd7" },
                    { 3, "High", "647609751afdaf2b05536cdf" },
                    { 4, "Medium", "647608041afdaf2b0545a16c" },
                    { 5, "High", "647608041afdaf2b0545a16b" },
                    { 6, "Low", "647608041afdaf2b0545a160" }
                });

            migrationBuilder.InsertData(
                table: "Scientists",
                columns: new[] { "Id", "Name", "TrelloMemberId", "TrelloToken" },
                values: new object[,]
                {
                    { 1, "Alessandro Ferluga", "5bf9f901921c336b20b29d25", "ATTA5c0a0bf47c1be3f495ebb81c42316684ff55e1134be71c0eba2cbecdd0614558CDCC81F8" },
                    { 2, "Marco de Piave", "639c692ed850f6055714fd55", "ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD" },
                    { 3, "Gabriele Ceccutti", "6474f28f0d4924c1eaff2824", "ATTA408bebeedb9948e62a1e38c11691049bc07e9329984c3897908a0127279faa4956E9CC86" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ExperimentId",
                table: "Comments",
                column: "ExperimentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ScientistId",
                table: "Comments",
                column: "ScientistId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TrelloId",
                table: "Comments",
                column: "TrelloId",
                unique: true,
                filter: "[TrelloId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_LabelId",
                table: "Experiments",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_ListId",
                table: "Experiments",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentScientist_ScientistsId",
                table: "ExperimentScientist",
                column: "ScientistsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "ExperimentScientist");

            migrationBuilder.DropTable(
                name: "Experiments");

            migrationBuilder.DropTable(
                name: "Scientists");

            migrationBuilder.DropTable(
                name: "EntityLists");

            migrationBuilder.DropTable(
                name: "Labels");
        }
    }
}
