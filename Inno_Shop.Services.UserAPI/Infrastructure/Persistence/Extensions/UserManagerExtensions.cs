using System.Linq.Dynamic.Core;
using Inno_Shop.Services.UserAPI.Infastructure.Persistence.Extensions.Utility;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;

namespace Inno_Shop.Services.UserAPI.Infastructure.Persistence.Extensions;

public static class UserManagerExtensions
{	
	public static IQueryable<User> Search(this IQueryable<User> users, string searchTerm)
	{
		if (string.IsNullOrWhiteSpace(searchTerm)) 
			return users;

		var lowerCaseTerm = searchTerm.Trim().ToLower();
        return users.Where(e => e.UserName!.ToLower().Contains(lowerCaseTerm));
    }

	public static IQueryable<User> Sort(this IQueryable<User> users, string orderByQueryString)
	{
		if (string.IsNullOrWhiteSpace(orderByQueryString))
			return users.OrderBy(e => e.UserName);

		var orderQuery = OrderQueryBuilder.CreateOrderQuery<User>(orderByQueryString);

		if (string.IsNullOrWhiteSpace(orderQuery))
			return users.OrderBy(e => e.UserName);

		return users.OrderBy(orderQuery);
	}
}
