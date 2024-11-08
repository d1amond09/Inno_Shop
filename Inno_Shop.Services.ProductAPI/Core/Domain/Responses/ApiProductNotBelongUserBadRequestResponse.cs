namespace Inno_Shop.Services.ProductAPI.Core.Domain.Responses;

public sealed class ApiProductNotBelongUserBadRequestResponse(Guid id, Guid userId ) : 
	ApiBadRequestResponse($"The product with id ({id}) doesn't belong to the current user with id ({userId}).")
{
}

