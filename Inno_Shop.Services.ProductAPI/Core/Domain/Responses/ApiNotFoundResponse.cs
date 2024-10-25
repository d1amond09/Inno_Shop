namespace Inno_Shop.Services.ProductAPI.Core.Domain.Responses;

public abstract class ApiNotFoundResponse(string message) : ApiBaseResponse(false)
{
	public string Message { get; set; } = message;
}
