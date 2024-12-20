﻿// <auto-generated />
using System;
using Inno_Shop.Services.ProductAPI.Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Inno_Shop.Services.ProductAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241110151303_ModelsToDatabase")]
    partial class ModelsToDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Inno_Shop.Services.ProductAPI.Domain.Models.Product", b =>
                {
                    b.Property<Guid>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("Availability")
                        .HasColumnType("bit");

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Price")
                        .IsRequired()
                        .HasColumnType("float");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ProductID");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductID = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                            Availability = true,
                            CategoryName = "Books",
                            CreationDate = new DateTime(2024, 11, 10, 18, 13, 2, 986, DateTimeKind.Local).AddTicks(9021),
                            Description = "A kind of antipode of the second great dystopia of the 20th century - \"Brave New World\" by Aldous Huxley. What is, in essence, more terrible: \"consumer society\" taken to the point of absurdity - or \"idea society\" taken to the absolute? According to Orwell, there is and cannot be anything more terrible than total lack of freedom...",
                            ImageUrl = "",
                            Name = "Book '1984' by George Orwell",
                            Price = 14.9,
                            UserID = new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2")
                        },
                        new
                        {
                            ProductID = new Guid("c9d4c053-49b2-430c-bc38-2d54a9991870"),
                            Availability = false,
                            CategoryName = "Books",
                            CreationDate = new DateTime(2024, 11, 10, 18, 13, 2, 986, DateTimeKind.Local).AddTicks(9040),
                            Description = "Among Shakespeare's plays, \"Hamlet\" is considered by many his masterpiece. Among actors, the role of Hamlet, Prince of Denmark, is considered the jewel in the crown of a triumphant theatrical career. Now Kenneth Branagh plays the leading role and co-directs a brillant ensemble performance. Three generations of legendary leading actors, many of whom first assembled for the Oscar-winning film \"Henry V\", gather here to perform the rarely heard complete version of the play. This clear, subtly nuanced, stunning dramatization, presented by The Renaissance Theatre Company in association with \"Bbc\" Broadcasting, features such luminaries as Sir John Gielgud, Derek Jacobi, Emma Thompson and Christopher Ravenscroft. It combines a full cast with stirring music and sound effects to bring this magnificent Shakespearen classic vividly to life. Revealing new riches with each listening, this production of \"Hamlet\" is an invaluable aid for students, teachers and all true lovers of Shakespeare - a recording to be treasured for decades to come.",
                            ImageUrl = "",
                            Name = "Book 'Hamlet' by William Shakespeare",
                            Price = 16.899999999999999,
                            UserID = new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2")
                        },
                        new
                        {
                            ProductID = new Guid("c9d4c053-49b1-111c-bc78-2d54a9991870"),
                            Availability = true,
                            CategoryName = "Computer mice",
                            CreationDate = new DateTime(2024, 11, 10, 18, 13, 2, 986, DateTimeKind.Local).AddTicks(9044),
                            Description = "LIGHTFORCE hybrid optical-mechanical primary switches, HERO 25K gaming sensor, compatible with PC - macOS/Windows - Black",
                            ImageUrl = "",
                            Name = "Logitech G502 X Wired Gaming Mouse",
                            Price = 149.90000000000001,
                            UserID = new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2")
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
