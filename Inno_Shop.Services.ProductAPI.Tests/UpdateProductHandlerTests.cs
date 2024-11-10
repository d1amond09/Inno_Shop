using Moq;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Application.Handlers;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;

namespace Inno_Shop.Services.ProductAPI.Tests;

public class UpdateProductHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateProductHandler _handler;

    public UpdateProductHandlerTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdateProductHandler(_mockRepo.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_InvalidUserId_ReturnsApiInvalidUserIdBadRequestResponse()
    {
        // Arrange
        var command = new UpdateProductCommand("invalid-user-id", Guid.NewGuid(), new ProductForUpdateDto(), false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<ApiInvalidUserIdBadRequestResponse>(result);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ReturnsProductNotFoundResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateProductCommand(userId.ToString(), Guid.NewGuid(), new ProductForUpdateDto(), true);

        _mockRepo.Setup(repo => repo.GetProductByIdAsync(command.Id, false))
                 .ReturnsAsync((Product)null); 

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<ProductNotFoundResponse>(result); 
    }

    [Fact]
    public async Task Handle_ProductDoesNotBelongToUser_ReturnsApiProductNotBelongUserBadRequestResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateProductCommand(userId.ToString(), Guid.NewGuid(), new ProductForUpdateDto(), true);

        var productEntity = new Product { UserID = Guid.NewGuid() }; 

        _mockRepo.Setup(repo => repo.GetProductByIdAsync(command.Id, true))
                 .ReturnsAsync(productEntity); 

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<ApiProductNotBelongUserBadRequestResponse>(result);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesProduct()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateProductCommand(userId.ToString(), Guid.NewGuid(), new ProductForUpdateDto() { Name = "Updated Product" }, true);

        var productEntity = new Product
        {
            ProductID = command.Id,
            UserID = userId, 
            Name = "Original Product"
        };

        _mockRepo.Setup(repo => repo.GetProductByIdAsync(command.Id, true))
                 .ReturnsAsync(productEntity); 

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var okResponse = Assert.IsType<ApiOkResponse<Product>>(result);
        Assert.NotNull(okResponse);
        Assert.Equal(productEntity, okResponse.Result);  
        _mockMapper.Verify(m => m.Map(command.Product, productEntity), Times.Once);
        _mockRepo.Verify(repo => repo.SaveAsync(), Times.Once); 
    }
}
