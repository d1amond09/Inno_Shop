using Inno_Shop.Services.ProductAPI.Models;

namespace Inno_Shop.Services.ProductAPI.Repository;

public class ProductRepository : IProductRepository
{
	public Task<Product> CreateUpdateProduct(Product product)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteProduct(Product product)
	{
		throw new NotImplementedException();
	}

	public Task<Product> GetProductById(int productId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<Product>> GetProducts()
	{
		throw new NotImplementedException();
	}
}
