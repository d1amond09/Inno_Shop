using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Azure.Core;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Inno_Shop.Services.UserAPI.Core.Application.Handlers;

public class CreateTokenHandler : IRequestHandler<CreateTokenCommand, string>
{
	private readonly UserManager<User> _userManager;
	private readonly IConfiguration _configuration;

	public CreateTokenHandler(UserManager<User> userManager, IConfiguration configuration)
	{
		_userManager = userManager;
		_configuration = configuration;
	}

	public async Task<string> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
	{
		var signingCredentials = GetSigningCredentials();
		var claims = await GetClaims(request.User);
		var tokenOptions = GenerateTokenOptions(signingCredentials, claims);



		return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
	}

	private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
	{
		var tokenOptions = new JwtSecurityToken(
			issuer: _configuration["Jwt:Issuer"],
			audience: _configuration["Jwt:Audience"],
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
			new Claim(ClaimTypes.Name, user.UserName)
		};

		var roles = await _userManager.GetRolesAsync(user);
		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		return claims;
	}
}

