using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ignite.Data.Migrations
{
    public partial class AddNameToClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Classes");
        }
    }
}
