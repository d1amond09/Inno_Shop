using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Application.Handlers;
using Inno_Shop.Services.ProductAPI.Core.Application.Queries;
using Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.ProductAPI.Core.Domain.Models;
using Inno_Shop.Services.ProductAPI.Core.Domain.RequestFeatures;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using Inno_Shop.Services.ProductAPI.Presentation.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Inno_Shop.Services.ProductAPI.Tests.Handlers;

public class GetProductsForUserHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IProductLinks> _mockProductLinks;
    private readonly GetProductsForUserHandler _handler;

    public GetProductsForUserHandlerTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockProductLinks = new Mock<IProductLinks>();
        _handler = new GetProductsForUserHandler(_mockRepo.Object, _mockMapper.Object, _mockProductLinks.Object);
    }

    [Fact]
    public async Task Handle_InvalidUserId_ReturnsApiInvalidUserIdBadRequestResponse()
    {
        // Arrange
        var query = new GetProductsForUserQuery(
           "invalid-guid",
            new LinkParameters(new ProductParameters(), new DefaultHttpContext()),
            false
        );

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsType<ApiInvalidUserIdBadRequestResponse>(result);
    }

    [Fact]
    public async Task Handle_InvalidPriceRange_ReturnsApiMaxPriceRangeBadRequestResponse()
    {
        // Arrange
        var query = new GetProductsForUserQuery(
            Guid.NewGuid().ToString(),
            new LinkParameters(
                new ProductParameters {
                    MaxPrice = -1 
                },
                new DefaultHttpContext()
            ),
            false
        );

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsType<ApiMaxPriceRangeBadRequestResponse>(result);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsApiOkResponseWithProducts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var products = new List<Product>
        {
            new() {
                ProductID = Guid.NewGuid(),
                Name = "Product 1",
                Description = "Description 1",
                Price = 25.99,
                Availability = true,
                CategoryName = "Category 1",
                CreationDate = DateTime.UtcNow,
                ImageUrl = "",
                UserID = userId
            },
            new() {
                ProductID = Guid.NewGuid(),
                Name = "Product 2",
                Description = "Description 2",
                Price = 15.49,
                Availability = true,
                CategoryName = "Category 2",
                CreationDate = DateTime.UtcNow,
                ImageUrl = "",
                UserID = userId
            }
        };

        var productDto = new List<ProductDto>
        {
            new() { ProductID = products[0].ProductID, Name = products[0].Name },
            new() { ProductID = products[1].ProductID, Name = products[1].Name }
        };

        var pagedList = new PagedList<Product>(products, products.Count, 1, 10);

        var productParameters = new ProductParameters
        {
            MaxPrice = 100,
            MinPrice = 10,
            OrderBy = "Price",
            PageNumber = 1,
            PageSize = 10
        };

        var httpContext = new DefaultHttpContext();

        var linkParameters = new LinkParameters(productParameters, httpContext);

        var query = new GetProductsForUserQuery(
            userId.ToString(),
            linkParameters,
            false
        );

        _mockRepo.Setup(repo => repo.GetProductsByUserIdAsync(userId, productParameters, false))
                 .ReturnsAsync(pagedList);

        _mockMapper.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productDto);

        var linkResponse = new LinkResponse
        {
            HasLinks = true,
            LinkedEntities = new LinkCollectionWrapper<Entity> {
                Value = [[],[]]
            },
            ShapedEntities = []
        };

        _mockProductLinks.Setup(links => links.TryGenerateLinks(productDto, "", httpContext))
                         .Returns(linkResponse);

        // Act
        var baseResult = await _handler.Handle(query, CancellationToken.None);
        var result = baseResult.GetResult<(LinkResponse, MetaData)>();

        // Assert
        var okResponse = Assert.IsType<(LinkResponse, MetaData)>(result);
        Assert.NotNull(okResponse);
        Assert.NotNull(okResponse.Item1);
        Assert.NotNull(okResponse.Item1.LinkedEntities); 
        Assert.NotEmpty(okResponse.Item1.LinkedEntities.Value); 
        Assert.Equal(productDto.Count, okResponse.Item2.TotalCount); 
    }
}
