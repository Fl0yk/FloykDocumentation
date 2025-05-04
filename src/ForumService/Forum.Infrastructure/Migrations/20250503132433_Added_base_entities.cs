using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_base_entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    normalized_username = table.Column<string>(type: "text", nullable: false),
                    public_username = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.id);
                    table.ForeignKey(
                        name: "FK_questions_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "answers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    text = table.Column<string>(type: "text", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answers", x => x.id);
                    table.ForeignKey(
                        name: "FK_answers_answers_parent_id",
                        column: x => x.parent_id,
                        principalTable: "answers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_answers_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_answers_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_answers_author_id",
                table: "answers",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_answers_is_deleted_id",
                table: "answers",
                columns: new[] { "is_deleted", "id" });

            migrationBuilder.CreateIndex(
                name: "IX_answers_parent_id",
                table: "answers",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_answers_QuestionId",
                table: "answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_questions_author_id",
                table: "questions",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_questions_is_deleted_id",
                table: "questions",
                columns: new[] { "is_deleted", "id" });

            migrationBuilder.CreateIndex(
                name: "IX_users_is_deleted_id",
                table: "users",
                columns: new[] { "is_deleted", "id" });

            migrationBuilder.CreateIndex(
                name: "IX_users_is_deleted_username",
                table: "users",
                columns: new[] { "is_deleted", "username" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answers");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
