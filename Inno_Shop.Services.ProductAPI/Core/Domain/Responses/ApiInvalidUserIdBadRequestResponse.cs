namespace Inno_Shop.Services.ProductAPI.Core.Domain.Responses;

public sealed class ApiInvalidUserIdBadRequestResponse(string? id ) : 
	ApiBadRequestResponse($"Invalid user ID: {id}")
{
}

