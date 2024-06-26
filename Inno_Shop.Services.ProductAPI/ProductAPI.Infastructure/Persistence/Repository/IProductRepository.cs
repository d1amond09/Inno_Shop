using Inno_Shop.Services.ProductAPI.Models;

namespace Inno_Shop.Services.ProductAPI.Repository;

public interface IProductRepository
{
	Task<IEnumerable<Product>> GetProducts();
	Task<Product> GetProductById(int productId);
	Task<Product> CreateUpdateProduct(Product product);
	Task<bool> DeleteProduct(Product product);
}
