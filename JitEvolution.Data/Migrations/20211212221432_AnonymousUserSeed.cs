using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitEvolution.Data.Migrations
{
    public partial class AnonymousUserSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "JitEvolution",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccessKey", "ChangedAt", "ChangedById", "ConcurrencyStamp", "CreatedAt", "CreatedById", "DeletedAt", "DeletedById", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("eedee617-fe65-43fe-9298-12dfbcbd9f07"), 0, null, new DateTime(2021, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("002810a2-3749-4809-856d-cee5adca656e"), "cb7b908b-e256-4433-b90d-f77cb32818e3", new DateTime(2021, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("002810a2-3749-4809-856d-cee5adca656e"), null, null, "anonymous@linde.ee", false, false, null, "ANONYMOUS@LINDE.EE", "ANONYMOUS", "AQAAAAEAACcQAAAAEHsMGzjMkGokHqRjfxWTubUykxdrCkvbl0dt148ZjbUpQjyMiCseYeXZNPoDHj5SEw==", null, false, "1695ade6-3492-46d4-8da7-e5e6bee0164b", false, "anonymous" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "JitEvolution",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("eedee617-fe65-43fe-9298-12dfbcbd9f07"));
        }
    }
}
