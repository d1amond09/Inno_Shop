using Inno_Shop.Services.ProductAPI.DbContexts;
using Inno_Shop.Services.ProductAPI.Models;
using Inno_Shop.Services.ProductAPI.ProductAPI.Core.ProductAPI.Application.Interfaces;

namespace Inno_Shop.Services.ProductAPI.Repository;

public class ProductRepository : IProductRepository
{
	public Task<bool> Create(Product product)
	{
		throw new NotImplementedException();
	}

	public Task<bool> Delete(Product product)
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

	public Task<bool> Update(Product product)
	{
		throw new NotImplementedException();
	}
}
