using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Article.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_base_entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_categories_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "categories",
                        principalColumn: "id");
                });

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
                name: "saved_articles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    article_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saved_articles", x => x.id);
                    table.ForeignKey(
                        name: "FK_saved_articles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "created_at", "deleted_at", "name", "order", "parent_id", "updated_at" },
                values: new object[,]
                {
                    { new Guid("21db0e42-32ba-4a0b-a126-620aad2b1091"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Base C#", 0, null, null },
                    { new Guid("8f140628-270e-48fd-b206-92610afb6510"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Platform .NET", 1, null, null },
                    { new Guid("8eb020a2-7f4f-4726-8b3b-b3614a474ec7"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Classes, structures and namespaces", 2, new Guid("21db0e42-32ba-4a0b-a126-620aad2b1091"), null },
                    { new Guid("af65248a-df56-456a-b84d-d1756ce06765"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "C# types and functions", 1, new Guid("21db0e42-32ba-4a0b-a126-620aad2b1091"), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_is_deleted_id",
                table: "categories",
                columns: new[] { "is_deleted", "id" });

            migrationBuilder.CreateIndex(
                name: "IX_categories_parent_id",
                table: "categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_saved_articles_is_deleted_id",
                table: "saved_articles",
                columns: new[] { "is_deleted", "id" });

            migrationBuilder.CreateIndex(
                name: "IX_saved_articles_is_deleted_user_id_article_id",
                table: "saved_articles",
                columns: new[] { "is_deleted", "user_id", "article_id" },
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "IX_saved_articles_user_id",
                table: "saved_articles",
                column: "user_id");

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
                name: "categories");

            migrationBuilder.DropTable(
                name: "saved_articles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
