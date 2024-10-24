using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Contracts;

public interface IProductService
{
	public Task<IEnumerable<ProductDto>> GetProducts(bool trackChanges);
}
