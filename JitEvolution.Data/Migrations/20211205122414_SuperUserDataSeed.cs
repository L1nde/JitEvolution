using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitEvolution.Data.Migrations
{
    public partial class SuperUserDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedById",
                schema: "JitEvolution",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.InsertData(
                schema: "JitEvolution",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ChangedAt", "ChangedById", "ConcurrencyStamp", "CreatedAt", "CreatedById", "DeletedAt", "DeletedById", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("002810a2-3749-4809-856d-cee5adca656e"), 0, new DateTime(2021, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("002810a2-3749-4809-856d-cee5adca656e"), "aaa20ff6-6bf2-4f3c-897f-6bf35568bf3b", new DateTime(2021, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("002810a2-3749-4809-856d-cee5adca656e"), null, null, "superuser@linde.ee", false, false, null, "SUPERUSER@LINDE.EE", "SUPERUSER", "AQAAAAEAACcQAAAAEHsMGzjMkGokHqRjfxWTubUykxdrCkvbl0dt148ZjbUpQjyMiCseYeXZNPoDHj5SEw==", null, false, "b3af7d63-7742-4060-85ba-dd41e57f6265", false, "superuser" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "AspNetUsers",
                column: "DeletedById",
                principalSchema: "JitEvolution",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                schema: "JitEvolution",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("002810a2-3749-4809-856d-cee5adca656e"));

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedById",
                schema: "JitEvolution",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "AspNetUsers",
                column: "DeletedById",
                principalSchema: "JitEvolution",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
