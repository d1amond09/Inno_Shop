namespace Inno_Shop.Services.ProductAPI.Core.Domain.Responses;

public sealed class ProductNotFoundResponse(Guid id) : 
	ApiNotFoundResponse($"Product with id: {id} is not found in db.")
{
}

