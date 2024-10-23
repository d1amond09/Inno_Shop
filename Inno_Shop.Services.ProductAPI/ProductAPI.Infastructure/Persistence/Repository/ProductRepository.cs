using Inno_Shop.Services.ProductAPI.Models;
using Inno_Shop.Services.ProductAPI.ProductAPI.Core.ProductAPI.Application.Interfaces;
using Inno_Shop.Services.ProductAPI.ProductAPI.Infastructure.Persistence;
using Inno_Shop.Services.ProductAPI.ProductAPI.Infastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop.Services.ProductAPI.Repository;

public class ProductRepository(AppDbContext db) : RepositoryBase<Product>(db), IProductRepository 
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

	public async Task<IEnumerable<Product>> GetProducts(bool trackChanges) =>
		await FindAll(trackChanges)
		.OrderBy(c => c.Name)
		.ToListAsync();

	public Task<bool> Update(Product product)
	{
		throw new NotImplementedException();
	}
}
