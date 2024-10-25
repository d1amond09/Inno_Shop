using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


namespace Inno_Shop.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
				columns: ["ProductID", "Availability", "CategoryName", "CreationDate", "Description", "ImageUrl", "Name", "Price"],
                values: new object[,]
                {
                    { new Guid("c9d4c053-49b1-111c-bc78-2d54a9991870"), true, "Computer mice", new DateTime(2024, 10, 25, 11, 52, 56, 706, DateTimeKind.Local).AddTicks(5803), "LIGHTFORCE hybrid optical-mechanical primary switches, HERO 25K gaming sensor, compatible with PC - macOS/Windows - Black", "", "Logitech G502 X Wired Gaming Mouse", 149.90000000000001 },
                    { new Guid("c9d4c053-49b2-430c-bc38-2d54a9991870"), false, "Books", new DateTime(2024, 10, 25, 11, 52, 56, 706, DateTimeKind.Local).AddTicks(5800), "Among Shakespeare's plays, \"Hamlet\" is considered by many his masterpiece. Among actors, the role of Hamlet, Prince of Denmark, is considered the jewel in the crown of a triumphant theatrical career. Now Kenneth Branagh plays the leading role and co-directs a brillant ensemble performance. Three generations of legendary leading actors, many of whom first assembled for the Oscar-winning film \"Henry V\", gather here to perform the rarely heard complete version of the play. This clear, subtly nuanced, stunning dramatization, presented by The Renaissance Theatre Company in association with \"Bbc\" Broadcasting, features such luminaries as Sir John Gielgud, Derek Jacobi, Emma Thompson and Christopher Ravenscroft. It combines a full cast with stirring music and sound effects to bring this magnificent Shakespearen classic vividly to life. Revealing new riches with each listening, this production of \"Hamlet\" is an invaluable aid for students, teachers and all true lovers of Shakespeare - a recording to be treasured for decades to come.", "", "Book 'Hamlet' by William Shakespeare", 16.899999999999999 },
                    { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), true, "Books", new DateTime(2024, 10, 25, 11, 52, 56, 706, DateTimeKind.Local).AddTicks(5777), "A kind of antipode of the second great dystopia of the 20th century - \"Brave New World\" by Aldous Huxley. What is, in essence, more terrible: \"consumer society\" taken to the point of absurdity - or \"idea society\" taken to the absolute? According to Orwell, there is and cannot be anything more terrible than total lack of freedom...", "", "Book '1984' by George Orwell", 14.9 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c9d4c053-49b1-111c-bc78-2d54a9991870"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c9d4c053-49b2-430c-bc38-2d54a9991870"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"));
        }
    }
}
