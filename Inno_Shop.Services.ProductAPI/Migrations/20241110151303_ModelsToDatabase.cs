using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Inno_Shop.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModelsToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Availability = table.Column<bool>(type: "bit", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "Availability", "CategoryName", "CreationDate", "Description", "ImageUrl", "Name", "Price", "UserID" },
                values: new object[,]
                {
                    { new Guid("c9d4c053-49b1-111c-bc78-2d54a9991870"), true, "Computer mice", new DateTime(2024, 11, 10, 18, 13, 2, 986, DateTimeKind.Local).AddTicks(9044), "LIGHTFORCE hybrid optical-mechanical primary switches, HERO 25K gaming sensor, compatible with PC - macOS/Windows - Black", "", "Logitech G502 X Wired Gaming Mouse", 149.90000000000001, new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2") },
                    { new Guid("c9d4c053-49b2-430c-bc38-2d54a9991870"), false, "Books", new DateTime(2024, 11, 10, 18, 13, 2, 986, DateTimeKind.Local).AddTicks(9040), "Among Shakespeare's plays, \"Hamlet\" is considered by many his masterpiece. Among actors, the role of Hamlet, Prince of Denmark, is considered the jewel in the crown of a triumphant theatrical career. Now Kenneth Branagh plays the leading role and co-directs a brillant ensemble performance. Three generations of legendary leading actors, many of whom first assembled for the Oscar-winning film \"Henry V\", gather here to perform the rarely heard complete version of the play. This clear, subtly nuanced, stunning dramatization, presented by The Renaissance Theatre Company in association with \"Bbc\" Broadcasting, features such luminaries as Sir John Gielgud, Derek Jacobi, Emma Thompson and Christopher Ravenscroft. It combines a full cast with stirring music and sound effects to bring this magnificent Shakespearen classic vividly to life. Revealing new riches with each listening, this production of \"Hamlet\" is an invaluable aid for students, teachers and all true lovers of Shakespeare - a recording to be treasured for decades to come.", "", "Book 'Hamlet' by William Shakespeare", 16.899999999999999, new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2") },
                    { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), true, "Books", new DateTime(2024, 11, 10, 18, 13, 2, 986, DateTimeKind.Local).AddTicks(9021), "A kind of antipode of the second great dystopia of the 20th century - \"Brave New World\" by Aldous Huxley. What is, in essence, more terrible: \"consumer society\" taken to the point of absurdity - or \"idea society\" taken to the absolute? According to Orwell, there is and cannot be anything more terrible than total lack of freedom...", "", "Book '1984' by George Orwell", 14.9, new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
