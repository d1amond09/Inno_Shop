using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;
using Inno_Shop.Services.ProductAPI.Core.Application.Handlers;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;

namespace Inno_Shop.Services.ProductAPI.Tests.Handlers;

public class DeleteProductHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly DeleteProductHandler _handler;

    public DeleteProductHandlerTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        _handler = new DeleteProductHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_InvalidUserId_ReturnsApiInvalidUserIdBadRequestResponse()
    {
        // Arrange
        var command = new DeleteProductCommand("invalid-guid", Guid.NewGuid(), false);

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
        var command = new DeleteProductCommand(userId.ToString(), Guid.NewGuid(), false);

        _mockRepo.Setup(repo => repo.GetProductByIdAsync(command.Id, command.TrackChanges)).ReturnsAsync((Product) null);

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
        var product = new Product
        {
            ProductID = Guid.NewGuid(),
            UserID = Guid.NewGuid() 
        };

        var command = new DeleteProductCommand(userId.ToString(), Guid.NewGuid(), false);

        _mockRepo.Setup(repo => repo.GetProductByIdAsync(command.Id, command.TrackChanges)).ReturnsAsync(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<ApiProductNotBelongUserBadRequestResponse>(result);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesProductSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var product = new Product
        {
            ProductID = Guid.NewGuid(),
            UserID = userId 
        };

        var command = new DeleteProductCommand(userId.ToString(), product.ProductID, false);

        _mockRepo.Setup(repo => repo.GetProductByIdAsync(command.Id, command.TrackChanges)).ReturnsAsync(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepo.Verify(repo => repo.DeleteProduct(product), Times.Once);
        _mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
        Assert.IsType<ApiOkResponse<Product>>(result);
    }
}