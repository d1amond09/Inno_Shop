namespace Inno_Shop.Services.ProductAPI.Core.Domain.Responses;

public abstract class ApiBaseResponse(bool success)
{
	public bool Success { get; set; } = success;
}
