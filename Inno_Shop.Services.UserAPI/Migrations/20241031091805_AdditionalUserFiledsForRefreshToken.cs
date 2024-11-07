using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inno_Shop.Services.UserAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalUserFiledsForRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e50abc84-786c-451f-8260-541a6405de87");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee6f298c-d41e-46ac-b765-4e61b4ebc554");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: ["Id", "ConcurrencyStamp", "Name", "NormalizedName"],
                values: new object[,]
                {
                    { "83029ce4-52d8-4648-a1d3-c2b31595d00d", null, "Administrator", "ADMINISTRATOR" },
                    { "a9aa6094-2b13-4ef3-8ef3-0234c1fe8c96", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83029ce4-52d8-4648-a1d3-c2b31595d00d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a9aa6094-2b13-4ef3-8ef3-0234c1fe8c96");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: ["Id", "ConcurrencyStamp", "Name", "NormalizedName"],
                values: new object[,]
                {
                    { "e50abc84-786c-451f-8260-541a6405de87", null, "User", "USER" },
                    { "ee6f298c-d41e-46ac-b765-4e61b4ebc554", null, "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
