using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("126719aa-4937-484f-a1ce-0151730e4457"),
                columns: new[] { "AuthorId", "TimeOfCreation" },
                values: new object[] { new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255c1"), new DateTime(2024, 12, 2, 0, 27, 11, 70, DateTimeKind.Local).AddTicks(5623) });

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("2a621bcb-01ae-4803-8584-fe0542fdee5c"),
                columns: new[] { "AuthorId", "TimeOfCreation" },
                values: new object[] { new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255c1"), new DateTime(2024, 12, 3, 0, 27, 11, 70, DateTimeKind.Local).AddTicks(5620) });

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("31ed4f16-6f88-42f4-bc9e-ede50898b39a"),
                columns: new[] { "AuthorId", "TimeOfCreation" },
                values: new object[] { new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255c9"), new DateTime(2024, 12, 5, 0, 27, 11, 70, DateTimeKind.Local).AddTicks(5617) });

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("5963ab4c-cb6f-4053-974e-2bd3da76ff6c"),
                columns: new[] { "AuthorId", "TimeOfCreation" },
                values: new object[] { new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255c8"), new DateTime(2024, 12, 4, 0, 27, 11, 70, DateTimeKind.Local).AddTicks(5613) });

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255ca"),
                columns: new[] { "AuthorId", "TimeOfCreation" },
                values: new object[] { new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255c7"), new DateTime(2024, 12, 3, 0, 27, 11, 70, DateTimeKind.Local).AddTicks(5599) });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                column: "DateOfCreation",
                value: new DateTime(2024, 11, 25, 21, 27, 11, 70, DateTimeKind.Utc).AddTicks(7414));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("25f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                column: "DateOfCreation",
                value: new DateTime(2024, 11, 5, 21, 27, 11, 70, DateTimeKind.Utc).AddTicks(7430));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("35f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                column: "DateOfCreation",
                value: new DateTime(2024, 12, 4, 21, 27, 11, 70, DateTimeKind.Utc).AddTicks(7437));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("45f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                column: "DateOfCreation",
                value: new DateTime(2024, 11, 30, 21, 27, 11, 70, DateTimeKind.Utc).AddTicks(7440));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Answers");

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("126719aa-4937-484f-a1ce-0151730e4457"),
                column: "TimeOfCreation",
                value: new DateTime(2024, 10, 12, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5527));

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("2a621bcb-01ae-4803-8584-fe0542fdee5c"),
                column: "TimeOfCreation",
                value: new DateTime(2024, 10, 13, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5524));

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("31ed4f16-6f88-42f4-bc9e-ede50898b39a"),
                column: "TimeOfCreation",
                value: new DateTime(2024, 10, 15, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5521));

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("5963ab4c-cb6f-4053-974e-2bd3da76ff6c"),
                column: "TimeOfCreation",
                value: new DateTime(2024, 10, 14, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5518));

            migrationBuilder.UpdateData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("a6c0936a-9d91-4d6a-b893-b257d5b255ca"),
                column: "TimeOfCreation",
                value: new DateTime(2024, 10, 13, 12, 54, 11, 650, DateTimeKind.Local).AddTicks(5502));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("15f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                column: "DateOfCreation",
                value: new DateTime(2024, 10, 6, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6815));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("25f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                column: "DateOfCreation",
                value: new DateTime(2024, 9, 16, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6819));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("35f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                column: "DateOfCreation",
                value: new DateTime(2024, 10, 15, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6832));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("45f010ed-8c38-4eeb-b9ec-5fb56ccf3189"),
                column: "DateOfCreation",
                value: new DateTime(2024, 10, 11, 9, 54, 11, 650, DateTimeKind.Utc).AddTicks(6835));
        }
    }
}
