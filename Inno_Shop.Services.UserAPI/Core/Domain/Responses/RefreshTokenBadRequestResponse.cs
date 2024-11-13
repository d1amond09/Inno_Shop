namespace Inno_Shop.Services.UserAPI.Core.Domain.Responses;

public sealed class RefreshTokenBadRequestResponse() : 
	ApiBadRequestResponse($"Invalid client request. The tokenDto has some invalid values.")
{
}

