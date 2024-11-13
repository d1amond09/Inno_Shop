using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Inno_Shop.Services.ProductAPI.Domain.Models;

namespace Inno_Shop.Services.ProductAPI.Infastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
	public void Configure(EntityTypeBuilder<Product> builder)
	{
		builder.HasData
		(
			new Product
			{
				ProductID = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
				Name = "Book '1984' by George Orwell",
				CategoryName = "Books",
				CreationDate = DateTime.Now,
				Price = 14.90,
				Availability = true,
				ImageUrl = "",
				Description = "A kind of antipode of the second great dystopia " +
							  "of the 20th century - \"Brave New World\" by Aldous Huxley. " +
							  "What is, in essence, more terrible: \"consumer society\" taken " +
							  "to the point of absurdity - or \"idea society\" taken to the " +
							  "absolute? According to Orwell, there is and cannot be anything " +
							  "more terrible than total lack of freedom...",
				UserID = new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2")
			},
			new Product
			{
				ProductID = new Guid("c9d4c053-49b2-430c-bc38-2d54a9991870"),
				Name = "Book 'Hamlet' by William Shakespeare",
				CategoryName = "Books",
				CreationDate = DateTime.Now,
				Price = 16.90,
				Availability = false,
				ImageUrl = "",
				Description = "Among Shakespeare's plays, \"Hamlet\" is considered by many his masterpiece. " +
							  "Among actors, the role of Hamlet, Prince of Denmark, is considered the jewel " +
							  "in the crown of a triumphant theatrical career. Now Kenneth Branagh plays the " +
							  "leading role and co-directs a brillant ensemble performance. Three generations " +
							  "of legendary leading actors, many of whom first assembled for the Oscar-winning " +
							  "film \"Henry V\", gather here to perform the rarely heard complete version of the " +
							  "play. This clear, subtly nuanced, stunning dramatization, presented by " +
							  "The Renaissance Theatre Company in association with \"Bbc\" Broadcasting, " +
							  "features such luminaries as Sir John Gielgud, Derek Jacobi, Emma Thompson and " +
							  "Christopher Ravenscroft. It combines a full cast with stirring music and sound " +
							  "effects to bring this magnificent Shakespearen classic vividly to life. Revealing " +
							  "new riches with each listening, this production of \"Hamlet\" is an invaluable aid " +
							  "for students, teachers and all true lovers of Shakespeare - a recording to be " +
							  "treasured for decades to come.",
                UserID = new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2")
            },
			new Product
			{
				ProductID = new Guid("c9d4c053-49b1-111c-bc78-2d54a9991870"),
				Name = "Logitech G502 X Wired Gaming Mouse",
				CategoryName = "Computer mice",
				CreationDate = DateTime.Now,
				Price = 149.90,
				Availability = true,
				ImageUrl = "",
				Description = "LIGHTFORCE hybrid optical-mechanical primary switches, " +
							  "HERO 25K gaming sensor, compatible with PC - macOS/Windows - Black",
                UserID = new Guid("b4f264b9-6772-48a8-b7f9-f6dae90559b2")
            }

		);
	}
}
