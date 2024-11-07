using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.Exceptions;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Inno_Shop.Services.UserAPI.Core.Application.Handlers;

public class RefreshTokenHandler(UserManager<User> userManager, IConfiguration configuration, ISender sender) : IRequestHandler<RefreshTokenCommand, TokenDto>
{
	private readonly UserManager<User> _userManager = userManager;
	private readonly IConfiguration _configuration = configuration;
    private readonly ISender _sender = sender;

    public async Task<TokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
	{
		var principal = GetPrincipalFromExpiredToken(request.TokenDto.AccessToken);
		var user = await _userManager.FindByNameAsync(principal.Identity.Name);
		if (user == null ||
			user.RefreshToken != request.TokenDto.RefreshToken ||
			user.RefreshTokenExpiryTime <= DateTime.Now)
			throw new RefreshTokenBadRequest();
        var tokenDto = await _sender.Send(new CreateTokenCommand(user, PopulateExp: false), cancellationToken);
        return tokenDto;
	}

	public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
	{
		var jwtSettings = _configuration.GetSection("JwtSettings");

		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = true,
			ValidateIssuer = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
			ValidateLifetime = true,
			ValidIssuer = jwtSettings["validIssuer"],
			ValidAudience = jwtSettings["validAudience"]
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

		if (securityToken is not JwtSecurityToken jwtSecurityToken ||
			!jwtSecurityToken.Header.Alg.Equals(
				SecurityAlgorithms.HmacSha256,
				StringComparison.InvariantCultureIgnoreCase)
			)
		{
			throw new SecurityTokenException("Invalid token");
		}

		return principal;
	}
}