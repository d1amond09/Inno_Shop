using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;

public record UserForUpdateDto
{
    public string? FirstName { get; init; }
	public string? LastName { get; init; }
	public string? UserName { get; init; }
    public string? Password { get; init; }
    public string? Email { get; init; }
	public string? PhoneNumber { get; init; }
}
