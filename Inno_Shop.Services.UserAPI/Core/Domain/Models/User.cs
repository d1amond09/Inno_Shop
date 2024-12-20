﻿using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Core.Domain.Models;

public class User : IdentityUser
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? RefreshToken { get; set; }
	public DateTime RefreshTokenExpiryTime { get; set; }
}
