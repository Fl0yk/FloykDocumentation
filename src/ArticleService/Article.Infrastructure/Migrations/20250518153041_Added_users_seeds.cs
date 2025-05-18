using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Article.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_users_seeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "deleted_at", "normalized_username", "public_username", "updated_at", "username" },
                values: new object[,]
                {
                    { new Guid("ac2d055a-4d0f-41d2-90f9-88393f1b65e7"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ADMIN", "Admin", null, "Admin" },
                    { new Guid("bb2d055a-4d0f-41d2-90f9-88393f1b65e7"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "AUTHOR", "Author", null, "Author" },
                    { new Guid("ff2d055a-4d0f-41d2-90f9-88393f1b65e7"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "FLOYK", "Floyk", null, "Floyk" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("ac2d055a-4d0f-41d2-90f9-88393f1b65e7"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("bb2d055a-4d0f-41d2-90f9-88393f1b65e7"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("ff2d055a-4d0f-41d2-90f9-88393f1b65e7"));
        }
    }
}
