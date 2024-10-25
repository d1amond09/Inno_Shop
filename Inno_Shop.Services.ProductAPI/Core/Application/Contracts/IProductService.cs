using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Contracts;

public interface IProductService
{
	Task<IEnumerable<ProductDto>> GetProductsAsync(bool trackChanges);
	Task<ProductDto> GetProductByIdAsync(Guid productId, bool trackChanges);
	Task<ProductDto> CreateProductAsync(ProductForCreationDto product);
	Task DeleteProductAsync(Guid productId, bool trackChanges);
	Task UpdateProductAsync(Guid productId, ProductForUpdateDto productForUpdate, bool trackChanges);
}
