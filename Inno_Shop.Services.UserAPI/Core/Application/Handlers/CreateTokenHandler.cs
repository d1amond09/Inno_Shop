using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Azure.Core;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Inno_Shop.Services.UserAPI.Core.Application.Handlers;

public class CreateTokenHandler(UserManager<User> userManager, IConfiguration configuration) : IRequestHandler<CreateTokenCommand, TokenDto>
{
	private readonly UserManager<User> _userManager = userManager;
	private readonly IConfiguration _configuration = configuration;

	public async Task<TokenDto> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
	{
		var signingCredentials = GetSigningCredentials();
		var claims = await GetClaims(request.User);
		var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

		var refreshToken = GenerateRefreshToken();

		request.User.RefreshToken = refreshToken;

		if (request.PopulateExp)
			request.User.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

		await _userManager.UpdateAsync(request.User);

		var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

		return new TokenDto(accessToken, refreshToken);
	}

	private string GenerateRefreshToken()
	{
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
	{
		var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:validIssuer"],
			audience: _configuration["JwtSettings:validAudience"],
            claims: claims,
			expires: DateTime.Now.AddMinutes(30),
			signingCredentials: signingCredentials
		);

		return tokenOptions;
	}

	private SigningCredentials GetSigningCredentials()
	{
		var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
		var secret = new SymmetricSecurityKey(key);
		return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
	}

	private async Task<List<Claim>> GetClaims(User user)
	{
		var claims = new List<Claim>
		{
			new (ClaimTypes.Name, user.UserName)
		};

		var roles = await _userManager.GetRolesAsync(user);
		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		return claims;
	}
}

