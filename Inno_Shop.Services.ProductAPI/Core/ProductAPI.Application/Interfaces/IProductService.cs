using Inno_Shop.Services.ProductAPI.Models.DataTransferObjects;

namespace Inno_Shop.Services.ProductAPI.ProductAPI.Core.ProductAPI.Application.Interfaces;

public interface IProductService
{
	public Task<IEnumerable<ProductDto>> GetProducts(bool trackChanges);
}
