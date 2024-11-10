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
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Core.Application.Queries;
using Inno_Shop.Services.ProductAPI.Presentation.Extensions;

namespace Inno_Shop.Services.ProductAPI.Tests;

public class GetProductHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetProductHandler _handler;

    public GetProductHandlerTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetProductHandler(_mockRepo.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ReturnsProductNotFoundResponse()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var query = new GetProductQuery(productId, false);

        _mockRepo.Setup(repo => repo.GetProductByIdAsync(productId, query.TrackChanges)).ReturnsAsync((Product)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsType<ProductNotFoundResponse>(result);
    }

    [Fact]
    public async Task Handle_ProductFound_ReturnsApiOkResponseWithProductDto()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            ProductID = productId,
            Name = "Sample Product",
            Description = "This is a sample product for testing.",
            Price = 49.99,
            Availability = true,
            CategoryName = "Sample Category",
            CreationDate = DateTime.UtcNow,
            ImageUrl = "",
            UserID = Guid.NewGuid()
        };

        var productDto = new ProductDto
        {
            ProductID = productId,
            Name = "Sample Product",
            Description = "This is a sample product for testing.",
            Price = 49.99,
            Availability = true,
            CategoryName = "Sample Category",
            CreationDate = DateTime.UtcNow,
            ImageUrl = "",
            UserID = product.UserID
        };

        var query = new GetProductQuery(productId, false);

        _mockRepo.Setup(repo => repo.GetProductByIdAsync(productId, query.TrackChanges)).ReturnsAsync(product);
        _mockMapper.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var okResponse = Assert.IsType<ApiOkResponse<ProductDto>>(result);
        Assert.Equal(productDto, okResponse.GetResult<ProductDto>());
    }
}
