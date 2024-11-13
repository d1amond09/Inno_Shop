namespace Inno_Shop.Services.UserAPI.Core.Domain.Responses;

public abstract class ApiBaseResponse(bool success)
{
	public bool Success { get; set; } = success;
}
