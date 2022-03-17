using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ignite.Data.Migrations
{
    public partial class RemoveIsSuspendedInEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuspended",
                table: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuspended",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
