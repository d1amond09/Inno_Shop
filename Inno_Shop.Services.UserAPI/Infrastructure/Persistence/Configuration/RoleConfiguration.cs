using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop.Services.UserAPI.Infrastructure.Persistence.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
	public void Configure(EntityTypeBuilder<IdentityRole> builder)
	{
		builder.HasData(
			new IdentityRole {
				Name = "Administrator",
				NormalizedName = "ADMINISTRATOR"
			},
			new IdentityRole
			{
				Name = "User",
				NormalizedName = "USER"
			}
		);
	}
}