using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitEvolution.Data.Migrations
{
    public partial class ChangeQueueItemUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QueueItem_ProjectId_IsActive",
                schema: "JitEvolution",
                table: "QueueItem");

            migrationBuilder.CreateIndex(
                name: "IX_QueueItem_ProjectId_IsActive",
                schema: "JitEvolution",
                table: "QueueItem",
                columns: new[] { "ProjectId", "IsActive" },
                unique: true,
                filter: "\"DeletedAt\" is null AND \"IsActive\" = true");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QueueItem_ProjectId_IsActive",
                schema: "JitEvolution",
                table: "QueueItem");

            migrationBuilder.CreateIndex(
                name: "IX_QueueItem_ProjectId_IsActive",
                schema: "JitEvolution",
                table: "QueueItem",
                columns: new[] { "ProjectId", "IsActive" },
                unique: true,
                filter: "\"DeletedAt\" is null");
        }
    }
}
