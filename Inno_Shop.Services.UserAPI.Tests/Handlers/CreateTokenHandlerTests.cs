using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Application.Handlers;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Core.Domain.ConfigurationModels;
using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using Inno_Shop.Services.UserAPI.Presentation.Extensions;

namespace Inno_Shop.Services.UserAPI.Tests.Handlers;

public class CreateTokenHandlerTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IOptionsMonitor<JwtConfiguration>> _mockJwtOptions;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly CreateTokenHandler _handler;

    public CreateTokenHandlerTests()
    {
        var storeMock = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(storeMock.Object, null, null, null, null, null, null, null, null);
        _mockJwtOptions = new Mock<IOptionsMonitor<JwtConfiguration>>();
        _mockConfig = new Mock<IConfiguration>();

        var jwtConfig = new JwtConfiguration
        {
            ValidIssuer = "http://localhost",
            ValidAudience = "http://localhost",
            Expires = "15"
        };

        _mockJwtOptions.Setup(m => m.Get("JwtSettings")).Returns(jwtConfig);
        _mockConfig.Setup(m => m["SECRET"]).Returns("my_super_secret_keymy_super_secret_keymy_super_secret_key");

        _handler = new CreateTokenHandler(_mockUserManager.Object, _mockJwtOptions.Object, _mockConfig.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesTokenSuccessfully()
    {
        var user = new User { Id = "user-id", UserName = "username" };
        var command = new CreateTokenCommand(user, true);

        _mockUserManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin", "User" });
        _mockUserManager.Setup(m => m.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        var result = await _handler.Handle(command, CancellationToken.None);

        var okResponse = Assert.IsType<ApiOkResponse<TokenDto>>(result);
        Assert.NotNull(okResponse);
        Assert.NotNull(okResponse.Result);
        Assert.Equal(user.RefreshToken, okResponse.Result.RefreshToken);
    }

    [Fact]
    public async Task Handle_UserUpdateFails_ReturnsApiOkResponse()
    {
        var user = new User { Id = "user-id", UserName = "username" };
        var command = new CreateTokenCommand(user, true);

        _mockUserManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(["Admin", "User"]);
        _mockUserManager.Setup(m => m.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error updating user." }));

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.IsType<ApiOkResponse<TokenDto>>(result);
    }
}