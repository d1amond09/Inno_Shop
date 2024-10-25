using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inno_Shop.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataAndUpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductID");

            migrationBuilder.AddColumn<bool>(
                name: "Availability",
                table: "Products",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Products",
                newName: "Id");
        }
    }
}
