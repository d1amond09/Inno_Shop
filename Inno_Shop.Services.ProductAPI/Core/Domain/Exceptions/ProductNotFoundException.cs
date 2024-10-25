namespace Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;

public class ProductNotFoundException(Guid productId) : 
	NotFoundException($"The product with id: {productId} doesn't exist in the database.")
{
}
