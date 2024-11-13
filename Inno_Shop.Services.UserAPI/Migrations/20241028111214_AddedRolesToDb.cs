using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inno_Shop.Services.UserAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedRolesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: ["Id", "ConcurrencyStamp", "Name", "NormalizedName"],
                values: new object[,]
                {
                    { "e50abc84-786c-451f-8260-541a6405de87", null, "User", "USER" },
                    { "ee6f298c-d41e-46ac-b765-4e61b4ebc554", null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e50abc84-786c-451f-8260-541a6405de87");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee6f298c-d41e-46ac-b765-4e61b4ebc554");
        }
    }
}
