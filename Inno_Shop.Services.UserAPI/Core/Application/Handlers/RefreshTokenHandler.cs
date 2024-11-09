using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.ConfigurationModels;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.Exceptions;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Inno_Shop.Services.UserAPI.Core.Application.Handlers;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, ApiBaseResponse>
{
    private readonly IOptionsMonitor<JwtConfiguration> _configuration;
	private readonly JwtConfiguration _jwtConfiguration;
	private readonly IConfiguration _config;
	private readonly UserManager<User> _userManager;
    private readonly ISender _sender;

    public RefreshTokenHandler(UserManager<User> userManager, IOptionsMonitor<JwtConfiguration> configuration, IConfiguration config, ISender sender)
    {
        _configuration = configuration;
        _jwtConfiguration = _configuration.Get("JwtSettings");
		_config = config;
        _userManager = userManager;
        _sender = sender;
    }

    public async Task<ApiBaseResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
	{
		var principal = GetPrincipalFromExpiredToken(request.TokenDto.AccessToken);
		var user = await _userManager.FindByNameAsync(principal.Identity?.Name!);
		
		if (user == null ||
			user.RefreshToken != request.TokenDto.RefreshToken ||
			user.RefreshTokenExpiryTime <= DateTime.Now)
			return new RefreshTokenBadRequestResponse();

        var tokenDto = await _sender.Send(new CreateTokenCommand(user, PopulateExp: false), cancellationToken);
        return tokenDto;
	}

	public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
	{
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = true,
			ValidateIssuer = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_config.GetValue<string>("SECRET")!)),
			ValidateLifetime = true,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience
        };

		var tokenHandler = new JwtSecurityTokenHandler();
		var principal = tokenHandler
			.ValidateToken(
				token, 
				tokenValidationParameters, 
				out SecurityToken securityToken);

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