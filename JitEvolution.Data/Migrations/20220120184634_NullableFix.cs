using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitEvolution.Data.Migrations
{
    public partial class NullableFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "Project");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedById",
                schema: "JitEvolution",
                table: "Project",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedById",
                schema: "JitEvolution",
                table: "AspNetRoles",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "AspNetRoles",
                column: "DeletedById",
                principalSchema: "JitEvolution",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "Project",
                column: "DeletedById",
                principalSchema: "JitEvolution",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "Project");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedById",
                schema: "JitEvolution",
                table: "Project",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedById",
                schema: "JitEvolution",
                table: "AspNetRoles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "AspNetRoles",
                column: "DeletedById",
                principalSchema: "JitEvolution",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_DeletedById",
                schema: "JitEvolution",
                table: "Project",
                column: "DeletedById",
                principalSchema: "JitEvolution",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
