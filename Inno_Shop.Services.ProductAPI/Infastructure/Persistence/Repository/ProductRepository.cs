using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using Inno_Shop.Services.ProductAPI.Infastructure.Persistence;
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

	public async Task<IEnumerable<Product>> GetProductsAsync(bool trackChanges) =>
		await FindAll(trackChanges)
		.OrderBy(c => c.Name)
		.ToListAsync();

	public Task SaveAsync() => _db.SaveChangesAsync();
}
