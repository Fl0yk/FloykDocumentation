using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedArticle_AspNetUsers_UserId",
                table: "SavedArticle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedArticle",
                table: "SavedArticle");

            migrationBuilder.RenameTable(
                name: "SavedArticle",
                newName: "SavedArticles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedArticles",
                table: "SavedArticles",
                columns: new[] { "UserId", "ArticleId" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ac2d055a-4d0f-41d2-90f9-88393f1b65e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d7988225-8562-4319-a33b-386e1b56635d", "AQAAAAIAAYagAAAAELJ0GZWN7kmV6QHUGsEjkmaF4fYutLZuzQh9CEVnXPj/eASyolvU8wED99BrlJKAzA==", "b0fd88b6-72cd-441e-8eb6-a091aaf9e40d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("bb2d055a-4d0f-41d2-90f9-88393f1b65e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "50020483-4c5a-4187-b181-b9f55e994da7", "AQAAAAIAAYagAAAAEEcg8XjPIudz/4bjQuWfQNHtI2V0KWB0IkR+oaWlczP6Pa6weVdMljMIbrDoEHHpGg==", "bbf70cee-9e20-441e-989f-14d398498633" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ff2d055a-4d0f-41d2-90f9-88393f1b65e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "673a9017-bda9-4ece-8155-fb8d9104195d", "AQAAAAIAAYagAAAAEBk+0NKV3B59Gbfc0/7t+WoRdsMxi9maX93dm5+h7K4gI/xhIR9DMActo0sVmW2KkA==", "627ba827-9b34-4fc3-a64b-7eca1e93fd30" });

            migrationBuilder.AddForeignKey(
                name: "FK_SavedArticles_AspNetUsers_UserId",
                table: "SavedArticles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedArticles_AspNetUsers_UserId",
                table: "SavedArticles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedArticles",
                table: "SavedArticles");

            migrationBuilder.RenameTable(
                name: "SavedArticles",
                newName: "SavedArticle");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedArticle",
                table: "SavedArticle",
                columns: new[] { "UserId", "ArticleId" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ac2d055a-4d0f-41d2-90f9-88393f1b65e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6cc5e3e5-4777-4804-881e-5ce58210aa9f", "AQAAAAIAAYagAAAAEAT/26EGLAGz1BHV6sredOHCB53dlKJLDXq6gzSsCVr050jguoR1QFpM7XztKxKtWQ==", "a97c250e-521e-46cf-bb2f-ab53fb65fe35" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("bb2d055a-4d0f-41d2-90f9-88393f1b65e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5efd3ae5-196c-4f19-bb57-3b0def5ac713", "AQAAAAIAAYagAAAAEAf62D8L6D1WaUEfB0rnD0tOBOZspYmj0Ooj8ZUAHrIkBeDWNYSdjRisT72m2OjReA==", "39f4fb34-85c7-46ce-b822-076356ebeeed" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ff2d055a-4d0f-41d2-90f9-88393f1b65e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad7126f7-faf0-407a-9049-7852b45ce8d6", "AQAAAAIAAYagAAAAEPFktBFiX4Ww8BHaXeZJyG0Td95H7mUmcfzyxeZGIIzm+rfCGJrrJ6acMwyLlaxU2g==", "e3d0fd73-09d2-42c5-8921-ef8a3117c805" });

            migrationBuilder.AddForeignKey(
                name: "FK_SavedArticle_AspNetUsers_UserId",
                table: "SavedArticle",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
