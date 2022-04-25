using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitEvolution.Data.Migrations
{
    public partial class AddedQueueItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QueueItem",
                schema: "JitEvolution",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ProjectFilePath = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedById = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueueItem_AspNetUsers_ChangedById",
                        column: x => x.ChangedById,
                        principalSchema: "JitEvolution",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueueItem_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "JitEvolution",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueueItem_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "JitEvolution",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QueueItem_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "JitEvolution",
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QueueItem_ChangedById",
                schema: "JitEvolution",
                table: "QueueItem",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_QueueItem_CreatedById",
                schema: "JitEvolution",
                table: "QueueItem",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QueueItem_DeletedById",
                schema: "JitEvolution",
                table: "QueueItem",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_QueueItem_ProjectId_IsActive",
                schema: "JitEvolution",
                table: "QueueItem",
                columns: new[] { "ProjectId", "IsActive" },
                unique: true,
                filter: "\"DeletedAt\" is null");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueueItem",
                schema: "JitEvolution");
        }
    }
}
