using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitEvolution.Data.Migrations
{
    public partial class ProjectEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Project",
                schema: "JitEvolution",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedById = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_AspNetUsers_ChangedById",
                        column: x => x.ChangedById,
                        principalSchema: "JitEvolution",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "JitEvolution",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "JitEvolution",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "JitEvolution",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_ChangedById",
                schema: "JitEvolution",
                table: "Project",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CreatedById",
                schema: "JitEvolution",
                table: "Project",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Project_DeletedById",
                schema: "JitEvolution",
                table: "Project",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Project_UserId_ProjectId",
                schema: "JitEvolution",
                table: "Project",
                columns: new[] { "UserId", "ProjectId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project",
                schema: "JitEvolution");
        }
    }
}
