using System.ComponentModel.Design;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Domain.RequestFeatures;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using Inno_Shop.Services.ProductAPI.Infastructure.Persistence;
using Inno_Shop.Services.ProductAPI.Infastructure.Persistence.Extensions;
using Inno_Shop.Services.ProductAPI.Infastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop.Services.ProductAPI.Repository;

public class ProductRepository(AppDbContext db) : RepositoryBase<Product>(db), IProductRepository
{
	public void CreateProduct(Product product) => Create(product);
	public void DeleteProduct(Product product) => Delete(product);

	public Task<Product?> GetProductByIdAsync(Guid productId, bool trackChanges) =>
		FindByCondition(c => c.ProductID.Equals(productId), trackChanges)
		.SingleOrDefaultAsync();

	public async Task<PagedList<Product>> GetProductsAsync(
		ProductParameters productParameters, 
		bool trackChanges)
	{
		var products = 
			await FindAll(trackChanges)
				.FilterProducts(productParameters.MinPrice, productParameters.MaxPrice)
				.Search(productParameters.SearchTerm)
				.Sort(productParameters.OrderBy)
				.Skip((productParameters.PageNumber - 1) * productParameters.PageSize)
				.Take(productParameters.PageSize)
				.ToListAsync();

		var count = await FindAll(trackChanges).CountAsync();

		return new PagedList<Product>(
			products, 
			count, 
			productParameters.PageNumber,
			productParameters.PageSize
		);
	}
		

	public Task SaveAsync() => _db.SaveChangesAsync();
}
