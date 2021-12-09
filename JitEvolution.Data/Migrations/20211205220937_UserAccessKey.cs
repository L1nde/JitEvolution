using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JitEvolution.Data.Migrations
{
    public partial class UserAccessKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessKey",
                schema: "JitEvolution",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessKey",
                schema: "JitEvolution",
                table: "AspNetUsers");
        }
    }
}
