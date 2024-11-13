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
using System.Linq;
using AutoMapper;
using Inno_Shop.Services.UserAPI.Core.Application.Queries;

namespace Inno_Shop.Services.UserAPI.Tests.Handlers;

public class GetUserHandlerTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetUserHandler _handler;

    public GetUserHandlerTests()
    {
        var storeMock = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(storeMock.Object, null, null, null, null, null, null, null, null);
        _mockMapper = new Mock<IMapper>();
        _handler = new GetUserHandler(_mockUserManager.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_UserExists_ReturnsUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId.ToString(), UserName = "username" };
        var userDto = new UserDto { Id = userId, UserName = "username" };
        var roles = new List<string> { "Admin", "User" };
        var query = new GetUserQuery(userId);

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);
        _mockUserManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(roles);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var okResponse = Assert.IsType<ApiOkResponse<UserDto>>(result);
        Assert.Equal(userDto, okResponse.Result);
        Assert.Equal(roles, okResponse.Result.Roles);
        _mockUserManager.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
        _mockUserManager.Verify(m => m.GetRolesAsync(user), Times.Once);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ReturnsUserNotFoundResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserQuery(userId);

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync((User)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var notFoundResponse = Assert.IsType<UserNotFoundResponse>(result);
        Assert.True(notFoundResponse.Message.Contains(userId.ToString()));
        _mockUserManager.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
        _mockUserManager.Verify(m => m.GetRolesAsync(It.IsAny<User>()), Times.Never);
    }
}
