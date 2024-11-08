using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inno_Shop.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserIDForProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c9d4c053-49b1-111c-bc78-2d54a9991870"),
                columns: new[] { "CreationDate", "UserID" },
                values: new object[] { new DateTime(2024, 11, 8, 8, 45, 0, 216, DateTimeKind.Local).AddTicks(4807), new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2") });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c9d4c053-49b2-430c-bc38-2d54a9991870"),
                columns: new[] { "CreationDate", "UserID" },
                values: new object[] { new DateTime(2024, 11, 8, 8, 45, 0, 216, DateTimeKind.Local).AddTicks(4803), new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2") });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                columns: new[] { "CreationDate", "UserID" },
                values: new object[] { new DateTime(2024, 11, 8, 8, 45, 0, 216, DateTimeKind.Local).AddTicks(4782), new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c9d4c053-49b1-111c-bc78-2d54a9991870"),
                column: "CreationDate",
                value: new DateTime(2024, 10, 25, 11, 52, 56, 706, DateTimeKind.Local).AddTicks(5803));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c9d4c053-49b2-430c-bc38-2d54a9991870"),
                column: "CreationDate",
                value: new DateTime(2024, 10, 25, 11, 52, 56, 706, DateTimeKind.Local).AddTicks(5800));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                column: "CreationDate",
                value: new DateTime(2024, 10, 25, 11, 52, 56, 706, DateTimeKind.Local).AddTicks(5777));
        }
    }
}
