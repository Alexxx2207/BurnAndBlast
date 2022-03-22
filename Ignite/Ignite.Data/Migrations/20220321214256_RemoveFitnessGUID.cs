using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ignite.Data.Migrations
{
    public partial class RemoveFitnessGUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Fitnesses_FitnessGuid",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_FitnessGuid",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "FitnessGuid",
                table: "Classes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FitnessGuid",
                table: "Classes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_FitnessGuid",
                table: "Classes",
                column: "FitnessGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Fitnesses_FitnessGuid",
                table: "Classes",
                column: "FitnessGuid",
                principalTable: "Fitnesses",
                principalColumn: "Guid");
        }
    }
}
