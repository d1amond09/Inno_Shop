using Inno_Shop.Services.ProductAPI.Domain.Models;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Contracts;

public interface IProductRepository
{
	public Task<IEnumerable<Product>> GetProducts(bool trackChanges);
	Task<Product> GetProductById(int productId);
}
