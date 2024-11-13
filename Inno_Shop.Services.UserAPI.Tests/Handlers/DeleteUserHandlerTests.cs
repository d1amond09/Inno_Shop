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

namespace Inno_Shop.Services.UserAPI.Tests.Handlers;

public class DeleteUserHandlerTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly DeleteUserHandler _handler;

    public DeleteUserHandlerTests()
    {
        var storeMock = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(storeMock.Object, null, null, null, null, null, null, null, null);
        _handler = new DeleteUserHandler(_mockUserManager.Object);
    }

    [Fact]
    public async Task Handle_UserExists_DeletesUserSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId.ToString(), UserName = "username" };
        var command = new DeleteUserCommand(userId);

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
        _mockUserManager.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var okResponse = Assert.IsType<ApiOkResponse<User>>(result);
        Assert.Equal(user, okResponse.Result);
        _mockUserManager.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
        _mockUserManager.Verify(m => m.DeleteAsync(user), Times.Once);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ReturnsUserNotFoundResponse()
    {
        // Arrange
        var userId = Guid.NewGuid(); 
        var command = new DeleteUserCommand(userId);

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync((User)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var notFoundResponse = Assert.IsType<UserNotFoundResponse>(result);
        Assert.True(notFoundResponse.Message.Contains(userId.ToString()));
        _mockUserManager.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
        _mockUserManager.Verify(m => m.DeleteAsync(It.IsAny<User>()), Times.Never);
    }
}
