﻿namespace Inno_Shop.Services.UserAPI.Core.Domain.Exceptions;

public sealed class RefreshTokenBadRequest : BadRequestException
{
	public RefreshTokenBadRequest()
	: base("Invalid client request. The tokenDto has some invalid values.")
	{
	}
}