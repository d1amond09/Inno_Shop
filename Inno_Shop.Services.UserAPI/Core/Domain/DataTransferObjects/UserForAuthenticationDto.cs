using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;

public record UserForAuthenticationDto
{
	[Required(ErrorMessage = "User name is required")]
	public string? UserName { get; init; }
	[Required(ErrorMessage = "Password name is required")]
	public string? Password { get; init; }
}
