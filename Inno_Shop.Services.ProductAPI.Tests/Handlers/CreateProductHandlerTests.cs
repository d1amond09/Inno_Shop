using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Moq;
using Inno_Shop.Services.ProductAPI.Core.Application.Handlers;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Presentation.Extensions;

namespace Inno_Shop.Services.ProductAPI.Tests.Handlers;

public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly IMapper _mapper;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        var config = new MapperConfiguration(cfg => { 
            cfg.CreateMap<ProductForCreationDto, Product>();
            cfg.CreateMap<Product, ProductDto>();
        });
        _mapper = config.CreateMapper();
        _handler = new CreateProductHandler(_mockRepo.Object, _mapper);
    }

    [Fact]
    public async Task Handle_InvalidUserId_ReturnsApiInvalidUserIdBadRequestResponse()
    {
        // Arrange
        var command = new CreateProductCommand("invalid-guid", new ProductForCreationDto());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<ApiInvalidUserIdBadRequestResponse>(result);
    }

    [Fact]
    public async Task Handle_ValidInput_CreatesProductSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var productForCreationDto = new ProductForCreationDto
        {
            Name = "Newly Created Product",
            Description = "Description for the newly created product.",
            Price = 49.99,
            Availability = true,
            CategoryName = "New Products",
            CreationDate = DateTime.UtcNow,
            ImageUrl = ""
        };
        var command = new CreateProductCommand(userId.ToString(), productForCreationDto);

        // Act
        var baseResult = await _handler.Handle(command, CancellationToken.None);
        var result = baseResult.GetResult<ProductDto>();

        // Assert
        _mockRepo.Verify(repo => repo.CreateProduct(It.IsAny<Product>()), Times.Once);
        _mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
        var okResponse = Assert.IsType<ProductDto>(result);
        Assert.NotNull(okResponse);
    }
}