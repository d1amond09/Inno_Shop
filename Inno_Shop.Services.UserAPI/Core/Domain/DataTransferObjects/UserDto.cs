using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;

public record UserDto
{
    public Guid Id { get; init; }
    public string? FirstName { get; init; }
	public string? LastName { get; init; }
	public string? UserName { get; init; }
	public string? Email { get; init; }
	public ICollection<string>? Roles { get; set; }
}
