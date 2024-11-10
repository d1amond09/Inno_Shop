using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Application.Handlers;
using Inno_Shop.Services.ProductAPI.Core.Application.Queries;
using Inno_Shop.Services.ProductAPI.Core.Domain.RequestFeatures;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using Microsoft.AspNetCore.Http;
using Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.ProductAPI.Core.Domain.Models;

namespace Inno_Shop.Services.ProductAPI.Tests;

public class GetProductsHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IProductLinks> _mockProductLinks;
    private readonly GetProductsHandler _handler;

    public GetProductsHandlerTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockProductLinks = new Mock<IProductLinks>();
        _handler = new GetProductsHandler(_mockRepo.Object, _mockMapper.Object, _mockProductLinks.Object);
    }

    [Fact]
    public async Task Handle_InvalidPriceRange_ReturnsApiMaxPriceRangeBadRequestResponse()
    {

        // Arrange
        var linkParameters = new LinkParameters(new ProductParameters() { MinPrice = 2, MaxPrice=1 }, new DefaultHttpContext());

        var query = new GetProductsQuery(linkParameters, false);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsType<ApiMaxPriceRangeBadRequestResponse>(result); 
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsApiOkResponseWithProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { ProductID = Guid.NewGuid(), Name = "Product 1", Price = 25.99 },
            new() { ProductID = Guid.NewGuid(), Name = "Product 2", Price = 15.49 }
        };

        var productParameters = new ProductParameters
        {
            MaxPrice = 100,
            MinPrice = 10,
            OrderBy = "Price",
            PageNumber = 1,
            PageSize = 10
        };

        var linkParameters = new LinkParameters(productParameters, new DefaultHttpContext());

        var query = new GetProductsQuery(linkParameters, false);

        var pagedList = new PagedList<Product>(products, products.Count, 1, 10);


        _mockRepo.Setup(repo => repo.GetProductsAsync(productParameters, false))
                 .ReturnsAsync(pagedList);

        var productDtos = new List<ProductDto>
        {
            new() { ProductID = products[0].ProductID, Name = products[0].Name },
            new() { ProductID = products[1].ProductID, Name = products[1].Name }
        };

        _mockMapper.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productDtos);

        var linkResponse = new LinkResponse
        {
            HasLinks = true,
            LinkedEntities = new LinkCollectionWrapper<Entity> { Value = [] },
            ShapedEntities = []
        };

        _mockProductLinks.Setup(links => links.TryGenerateLinks(productDtos, "", linkParameters.Context))
                         .Returns(linkResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var okResponse = Assert.IsType<ApiOkResponse<(LinkResponse, MetaData)>>(result);
        Assert.NotNull(okResponse);
        Assert.NotNull(okResponse.Result);
        Assert.NotNull(okResponse.Result.Item1); 
        Assert.Equal(products.Count, okResponse.Result.Item2.TotalCount); 
    }

    [Fact]
    public async Task Handle_NoProducts_ReturnsApiOkResponseWithEmptyProducts()
    {
        // Arrange
        var products = new List<Product>();

        var productParameters = new ProductParameters
        {
            MaxPrice = 100,
            MinPrice = 10,
            OrderBy = "Price",
            PageNumber = 1,
            PageSize = 10
        };

        var linkParameters = new LinkParameters(productParameters, new DefaultHttpContext());

        var query = new GetProductsQuery(linkParameters, false);

        var pagedList = new PagedList<Product>(products, products.Count, 1, 10);

        _mockRepo.Setup(repo => repo.GetProductsAsync(productParameters, false))
                 .ReturnsAsync(pagedList);

        _mockMapper.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns([]);

        var linkResponse = new LinkResponse
        {
            HasLinks = false, 
            LinkedEntities = new LinkCollectionWrapper<Entity> { Value = [] },
            ShapedEntities = []
        };

        _mockProductLinks.Setup(links => links.TryGenerateLinks(It.IsAny<IEnumerable<ProductDto>>(), "", linkParameters.Context))
                         .Returns(linkResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var okResponse = Assert.IsType<ApiOkResponse<(LinkResponse, MetaData)>>(result);
        Assert.NotNull(okResponse);
        Assert.NotNull(okResponse.Result);
        Assert.NotNull(okResponse.Result.Item1); 
        Assert.Empty(okResponse.Result.Item1.LinkedEntities.Value); 
        Assert.Equal(0, okResponse.Result.Item2.TotalCount);
    }
}