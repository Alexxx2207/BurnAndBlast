using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ignite.Data.Migrations
{
    public partial class DescriptionToEventsAndClassesAndPriceToClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuspended",
                table: "Classes");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Classes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Classes");

            migrationBuilder.AddColumn<bool>(
                name: "IsSuspended",
                table: "Classes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
