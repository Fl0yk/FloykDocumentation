using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_base_entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    refresh_token = table.Column<string>(type: "text", nullable: true),
                    refresh_token_expiry = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    public_username = table.Column<string>(type: "text", nullable: false),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    username = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    normalized_username = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enambled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "followings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_followings", x => x.id);
                    table.ForeignKey(
                        name: "FK_followings_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_followings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "ConcurrencyStamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("ba74ad5b-68da-4823-9a77-a424b56dac04"), null, "Admin", "ADMIN" },
                    { new Guid("fe2d04aa-4d0f-41d2-90f9-88393f1b65e7"), null, "Author", "AUTHOR" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "access_failed_count", "avatar", "concurrencyStamp", "created_at", "deleted_at", "email", "email_confirmed", "lockout_enambled", "lockout_end", "NormalizedEmail", "normalized_username", "password_hash", "public_username", "refresh_token", "refresh_token_expiry", "security_stamp", "updated_at", "username" },
                values: new object[,]
                {
                    { new Guid("ac2d055a-4d0f-41d2-90f9-88393f1b65e7"), 0, null, "20dc8e0c-f8c3-44bb-9a96-c97036e69f28", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "admin@mail.ru", true, false, null, "ADMIN@MAIL.RU", "ADMIN", "AQAAAAIAAYagAAAAEORF8UXpi2Jqi5POUjc74E8IGfCJI8AlAd2U9v2ZmSD6ZYnUfbt3aubyUIAJ5dcYlQ==", "Admin", null, null, "52a422fe-ddc2-49f3-9e3c-9b7d808d372d", null, "Admin" },
                    { new Guid("bb2d055a-4d0f-41d2-90f9-88393f1b65e7"), 0, null, "4b61a6d2-cbcd-4fb0-bc10-12c565b9cb6c", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "aauthor@mail.ru", true, false, null, "AUTHOR@MAIL.RU", "AUTHOR", "AQAAAAIAAYagAAAAEEH2LmXhCqPACPLripnmMf+rql5Q/+3BXmGfPO3j0qedIRsm5+PvhOHBPOeTKy1BIw==", "Author", null, null, "c038c298-0d1f-49eb-9c41-171ab2383440", null, "Author" },
                    { new Guid("ff2d055a-4d0f-41d2-90f9-88393f1b65e7"), 0, null, "046d944e-4036-4046-b392-f37e3ae7d535", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "kosach@mail.ru", true, false, null, "KOSACH@MAIL.RU", "FLOYK", "AQAAAAIAAYagAAAAEOde4ESweARTjnOnjr/oKjbLzIduMdPrYwaF8tpfysBi+BYBs0aozGkQY1MBqXwHLg==", "Floyk", null, null, "f2955a96-7110-482a-9c77-d0020f81e5c0", null, "Floyk" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("ba74ad5b-68da-4823-9a77-a424b56dac04"), new Guid("ac2d055a-4d0f-41d2-90f9-88393f1b65e7") },
                    { new Guid("fe2d04aa-4d0f-41d2-90f9-88393f1b65e7"), new Guid("ac2d055a-4d0f-41d2-90f9-88393f1b65e7") },
                    { new Guid("ba74ad5b-68da-4823-9a77-a424b56dac04"), new Guid("bb2d055a-4d0f-41d2-90f9-88393f1b65e7") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_followings_author_id",
                table: "followings",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_followings_is_deleted_id",
                table: "followings",
                columns: new[] { "is_deleted", "id" });

            migrationBuilder.CreateIndex(
                name: "IX_followings_is_deleted_user_id_author_id",
                table: "followings",
                columns: new[] { "is_deleted", "user_id", "author_id" });

            migrationBuilder.CreateIndex(
                name: "IX_followings_user_id",
                table: "followings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_users_is_deleted_id",
                table: "users",
                columns: new[] { "is_deleted", "id" });

            migrationBuilder.CreateIndex(
                name: "IX_users_is_deleted_normalized_username",
                table: "users",
                columns: new[] { "is_deleted", "normalized_username" },
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "IX_users_is_deleted_public_username",
                table: "users",
                columns: new[] { "is_deleted", "public_username" });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "users",
                column: "normalized_username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "followings");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
