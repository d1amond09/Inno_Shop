using System.ComponentModel.Design;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.ProductAPI.Core.Domain.Models;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Microsoft.Net.Http.Headers;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Utility;

public class ProductLinks(LinkGenerator linkGenerator, IDataShaper<ProductDto> dataShaper) : IProductLinks
{
    private readonly LinkGenerator _linkGenerator = linkGenerator;
    private readonly IDataShaper<ProductDto> _dataShaper = dataShaper;

    public LinkResponse TryGenerateLinks(IEnumerable<ProductDto> productsDto, string fields, HttpContext httpContext)
    {
        var shapedProducts = ShapeData(productsDto, fields);

        if (ShouldGenerateLinks(httpContext))
            return ReturnLinkdedProducts(productsDto, fields, httpContext, shapedProducts);

        return ReturnShapedProducts(shapedProducts);
    }

    private LinkResponse ReturnShapedProducts(List<Entity> shapedProducts) =>
        new() { ShapedEntities = shapedProducts };

    private List<Entity> ShapeData(IEnumerable<ProductDto> productsDto, string fields) =>
        _dataShaper.ShapeData(productsDto, fields)
            .Select(e => e.Entity)
            .ToList();

    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaType = (MediaTypeHeaderValue?) httpContext.Items["AcceptHeaderMediaType"];
        ArgumentNullException.ThrowIfNull(mediaType);
        return mediaType.SubTypeWithoutSuffix
            .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
    }
    private LinkResponse ReturnLinkdedProducts(IEnumerable<ProductDto> productsDto, string fields, HttpContext httpContext, List<Entity> shapedProducts)
    {
        var productDtoList = productsDto.ToList();

        for (var index = 0; index < productDtoList.Count; index++)
        {
            var productLinks = CreateLinksForProducts(httpContext, productDtoList[index].ProductID, fields);
            shapedProducts[index].Add("Links", productLinks);
        }

        var productCollection = new LinkCollectionWrapper<Entity>(shapedProducts);
        var linkedEmployees = CreateLinksForProducts(httpContext, productCollection);

        return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
    }

    private List<Link> CreateLinksForProducts(HttpContext httpContext, Guid id, string fields = "")
    {
        List<Link> links = [
            new Link(_linkGenerator.GetUriByAction(httpContext, "GetProduct", values: new { id, fields })!, "self", "GET"),
            new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteProduct", values: new { id })!, "delete_product", "DELETE"),
            new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateProduct", values: new { id })!, "update_product", "PUT")
        ];

        return links;
    }
    private LinkCollectionWrapper<Entity> CreateLinksForProducts(HttpContext httpContext, LinkCollectionWrapper<Entity> employeesWrapper)
    {
        employeesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetProduct", values: new { })!, "self", "GET"));
        return employeesWrapper;
    }
}
