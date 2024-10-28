namespace Inno_Shop.Services.UserAPI.Core.Domain.Responses;

public sealed class UserNotFoundResponse(Guid id) : 
	ApiNotFoundResponse($"User with id: {id} is not found in db.")
{
}

