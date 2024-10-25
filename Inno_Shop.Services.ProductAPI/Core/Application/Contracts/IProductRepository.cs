using Inno_Shop.Services.ProductAPI.Domain.Models;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Contracts;

public interface IProductRepository
{
	Task<IEnumerable<Product>> GetProductsAsync(bool trackChanges);
	Task<Product?> GetProductByIdAsync(Guid productId, bool trackChanges);
	void CreateProduct(Product product);
	void DeleteProduct(Product product);
	Task SaveAsync();
}
