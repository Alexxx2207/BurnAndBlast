using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ignite.Data.Migrations
{
    public partial class RemoveCompositeKeyUsersSubscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersSubscriptions",
                table: "UsersSubscriptions");

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionOrderId",
                table: "UsersSubscriptions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersSubscriptions",
                table: "UsersSubscriptions",
                column: "SubscriptionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersSubscriptions_UserId",
                table: "UsersSubscriptions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersSubscriptions",
                table: "UsersSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_UsersSubscriptions_UserId",
                table: "UsersSubscriptions");

            migrationBuilder.DropColumn(
                name: "SubscriptionOrderId",
                table: "UsersSubscriptions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersSubscriptions",
                table: "UsersSubscriptions",
                columns: new[] { "UserId", "SubscriptionId" });
        }
    }
}
