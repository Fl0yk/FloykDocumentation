using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forum.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Answers_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "AuthorId", "DateOfCreation", "Description", "IsClosed", "Title" },
                values: new object[,]
                {
                    { new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), new Guid("24f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), new DateTime(2024, 10, 6, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6815), "Description 1", false, "Title 1" },
                    { new Guid("25f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), new Guid("25f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), new DateTime(2024, 9, 16, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6819), "Description 2", false, "Title 2" },
                    { new Guid("35f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), new Guid("26f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), new DateTime(2024, 10, 15, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6832), "Description 3", false, "Title 3" },
                    { new Guid("45f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), new Guid("26f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), new DateTime(2024, 10, 11, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6835), "Description 4", false, "Title 4" }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "Level", "ParentId", "QuestionId", "Text", "TimeOfCreation" },
                values: new object[,]
                {
                    { new Guid("126719aa-4937-484f-a1ce-0151730e4457"), 0, null, new Guid("45f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), "Text 2", new DateTime(2024, 10, 12, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5527) },
                    { new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255ca"), 0, null, new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), "Text 1", new DateTime(2024, 10, 13, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5502) },
                    { new Guid("2a621bcb-01ae-4803-8584-fe0542fdee5c"), 1, new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255ca"), new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), "Text 1-2", new DateTime(2024, 10, 13, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5524) },
                    { new Guid("5963ab4c-cb6f-4053-974e-2bd3da76ff6c"), 1, new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255ca"), new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), "Text 1-1", new DateTime(2024, 10, 14, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5518) },
                    { new Guid("31ed4f16-6f88-42f4-bc9e-ede50898b39a"), 2, new Guid("5963ab4c-cb6f-4053-974e-2bd3da76ff6c"), new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"), "Text 1-1-1", new DateTime(2024, 10, 15, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5521) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_ParentId",
                table: "Answers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
