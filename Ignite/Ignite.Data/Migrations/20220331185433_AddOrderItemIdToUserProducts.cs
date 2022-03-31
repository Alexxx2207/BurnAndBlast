using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ignite.Data.Migrations
{
    public partial class AddOrderItemIdToUserProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersProducts",
                table: "UsersProducts");

            migrationBuilder.AddColumn<string>(
                name: "OrderItemId",
                table: "UsersProducts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersProducts",
                table: "UsersProducts",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersProducts_UserId",
                table: "UsersProducts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersProducts",
                table: "UsersProducts");

            migrationBuilder.DropIndex(
                name: "IX_UsersProducts_UserId",
                table: "UsersProducts");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "UsersProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersProducts",
                table: "UsersProducts",
                columns: new[] { "UserId", "ProductId" });
        }
    }
}
