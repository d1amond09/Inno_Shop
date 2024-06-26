using Inno_Shop.Services.ProductAPI.ProductAPI.Core.ProductAPI.Application.Interfaces;

namespace Inno_Shop.Services.ProductAPI.ProductAPI.Core.ProductAPI.Application.Services;

public class ProductService(IProductRepository rep)
{
	private readonly IProductRepository _rep = rep;
}
