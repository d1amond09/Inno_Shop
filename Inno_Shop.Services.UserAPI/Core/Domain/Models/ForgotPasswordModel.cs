using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.Services.UserAPI.Core.Domain.Models;

public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}
