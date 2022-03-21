using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ignite.Data.Migrations
{
    public partial class RemoveFitnessAddAddressToClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Fitnesses_FitnessId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_FitnessId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "FitnessId",
                table: "Classes");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Fitnesses_FitnessGuid",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_FitnessGuid",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "FitnessGuid",
                table: "Classes");

            migrationBuilder.AddColumn<string>(
                name: "FitnessId",
                table: "Classes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_FitnessId",
                table: "Classes",
                column: "FitnessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Fitnesses_FitnessId",
                table: "Classes",
                column: "FitnessId",
                principalTable: "Fitnesses",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
