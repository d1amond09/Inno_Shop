using Inno_Shop.Services.ProductAPI.Domain.Models;

namespace Inno_Shop.Services.ProductAPI.Infastructure.Persistence.Extensions;

public static class RepositoryProductExtensions
{
	public static IQueryable<Product> FilterProducts(this IQueryable<Product> products, double minPrice, double maxPrice) => 
		products.Where(e => (e.Price >= minPrice && e.Price <= maxPrice));
	
	public static IQueryable<Product> Search(this IQueryable<Product> products, string searchTerm)
	{
		if (string.IsNullOrWhiteSpace(searchTerm)) 
			return products;

		var lowerCaseTerm = searchTerm.Trim().ToLower();
		return products.Where(e => e.Name!.ToLower().Contains(lowerCaseTerm));
	}
}
