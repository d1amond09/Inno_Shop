using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;

public record UserForRegistrationDto
{
	public string? FirstName { get; init; }
	public string? LastName { get; init; }
	[Required(ErrorMessage = "Username is required")]
	public string? UserName { get; init; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
	public string? Password { get; init; }
    
	[DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }
    public string? Confirm { get; init; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string? Email { get; init; }
	public string? PhoneNumber { get; init; }
}
