using Inno_Shop.Services.ProductAPI.Models;

namespace Inno_Shop.Services.ProductAPI.ProductAPI.Core.ProductAPI.Application.Interfaces;

public interface IProductRepository
{
	Task<IEnumerable<Product>> GetProducts();
	Task<Product> GetProductById(int productId);
	Task<bool> Create(Product product);
	Task<bool> Update(Product product);
	Task<bool> Delete(Product product);
}
