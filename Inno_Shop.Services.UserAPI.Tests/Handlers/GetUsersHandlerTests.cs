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

namespace Inno_Shop.Services.UserAPI.Tests.Handlers;

public class YourDbContext : IdentityDbContext<User>
{
    public YourDbContext(DbContextOptions<YourDbContext> options) : base(options) { }
}

// Ваши классы User и другие определения

public class GetUsersHandlerTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUserLinks> _mockUserLinks;
    private readonly DbContextOptions<YourDbContext> _options;

    public GetUsersHandlerTests()
    {
        _options = new DbContextOptionsBuilder<YourDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _mockMapper = new Mock<IMapper>();
        _mockUserLinks = new Mock<IUserLinks>();
    }

    private DbContextOptions<YourDbContext> CreateNewContextOptions()
    {
        var options = new DbContextOptionsBuilder<YourDbContext>()
            .UseInMemoryDatabase("TestDatabase_" + Guid.NewGuid().ToString())
            .Options;
        return options;
    }


    [Fact]
    public async Task Handle_ReturnsEmptyResult_WhenNoUsersExist()
    {
        var options = CreateNewContextOptions();
        using var context = new YourDbContext(options);
        // Arrange
        var store = new UserStore<User>(context);
        var userManager = new UserManager<User>(
            store,
            new OptionsWrapper<IdentityOptions>(new IdentityOptions()),
            new PasswordHasher<User>(),
            null,
            null,
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null,
            null);

        var handler = new GetUsersHandler(userManager, _mockMapper.Object, _mockUserLinks.Object);
        var userParameters = new UserParameters
        {
            PageSize = 5,
            PageNumber = 1,
            OrderBy = "UserName",
        };

        var query = new GetUsersQuery(new LinkParameters(userParameters, new DefaultHttpContext()));


        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var okResponse = Assert.IsType<ApiOkResponse<(LinkResponse, MetaData)>>(result);
        Assert.NotNull(okResponse.Result);
        Assert.Equal(0, okResponse.Result.Item2.TotalCount);
    }

    [Fact]
    public async Task Handle_ReturnsPagedUsers_WhenUsersExist()
    {
        var options = CreateNewContextOptions();
        using var context = new YourDbContext(options);
        var users = new List<User>
            {
                new User { Id = "1", UserName = "User1" },
                new User { Id = "2", UserName = "User2" },
                new User { Id = "3", UserName = "User3" },
                new User { Id = "4", UserName = "User4" },
                new User { Id = "5", UserName = "User5" }
            };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        // Создаем UserStore
        var store = new UserStore<User>(context);

        // Создаем UserManager с реальным UserStore
        var userManager = new UserManager<User>(
            store,
            new OptionsWrapper<IdentityOptions>(new IdentityOptions()),
            new PasswordHasher<User>(),
            null,
            null,
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null,
            null);

        var handler = new GetUsersHandler(userManager, _mockMapper.Object, _mockUserLinks.Object);
        var userParameters = new UserParameters
        {
            PageSize = 5,
            PageNumber = 1,
            OrderBy = "UserName",
        };

        var query = new GetUsersQuery(new LinkParameters(userParameters, new DefaultHttpContext()));

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        var okResponse = Assert.IsType<ApiOkResponse<(LinkResponse, MetaData)>>(result);
        Assert.NotNull(okResponse.Result);
        Assert.Equal(users.Count, okResponse.Result.Item2.TotalCount);
    }

}
