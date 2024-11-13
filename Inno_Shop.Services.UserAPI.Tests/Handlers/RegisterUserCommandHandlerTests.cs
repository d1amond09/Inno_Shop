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
using Inno_Shop.Services.UserAPI.Core.Application.Contracts;
using Inno_Shop.Services.UserAPI.Core.Domain.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Inno_Shop.Services.UserAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.UserAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inno_Shop.Services.UserAPI.Tests.Handlers;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserPasswordStore<User>> _userPasswordStoreMock;
    private readonly Mock<IUserRoleStore<User>> _userRoleStoreMock;
    private readonly Mock<ILogger<UserManager<User>>> _loggerMock;
    private readonly UserManager<User> _userManagerMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _userPasswordStoreMock = new Mock<IUserPasswordStore<User>>();
        _userRoleStoreMock = new Mock<IUserRoleStore<User>>();
        _loggerMock = new Mock<ILogger<UserManager<User>>>();

        _userManagerMock = new UserManager<User>(
            _userPasswordStoreMock.Object,
            null,
            new PasswordHasher<User>(),
            null,
            null,
            null,
            null,
            null,
            _loggerMock.Object
        );

        _userRoleStoreMock.Setup(x => x.GetRolesAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new string[] { }); 

        _userRoleStoreMock.Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userPasswordStoreMock.Setup(x => x.GetUserIdAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("1");

        _mapperMock = new Mock<IMapper>();
        _handler = new RegisterUserCommandHandler(_mapperMock.Object, _userManagerMock);
    }


    [Fact]
    public async Task Handle_UserCreationFails_ReturnsApiBaseResponseWithErrors()
    {
        // Arrange
        var password = "Test123";  
        var userForRegistrationDto = new UserForRegistrationDto
        {
            Password = password,
            Roles = ["User"],
            ConfirmPassword = password,
            Email = ""
        };

        var registerUserCommand = new RegisterUserCommand(userForRegistrationDto);


        var user = new User();
        _mapperMock.Setup(m => m.Map<User>(It.IsAny<UserForRegistrationDto>())).Returns(user);

        var identityResult = IdentityResult.Failed(new IdentityError { Description = "User creation failed." });

        _userPasswordStoreMock.Setup(us => us.CreateAsync(user, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(identityResult);

        // Act
        var result = await _handler.Handle(registerUserCommand, CancellationToken.None);

        // Assert
        var idResult = result.GetResult<(IdentityResult, User)>().Item1;
        Assert.Equal(identityResult, idResult);

    }
}
