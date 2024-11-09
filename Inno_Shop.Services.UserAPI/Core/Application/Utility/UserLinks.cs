using System.ComponentModel.Design;
using Inno_Shop.Services.UserAPI.Core.Application.Contracts;
using Inno_Shop.Services.UserAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Microsoft.Net.Http.Headers;

namespace Inno_Shop.Services.UserAPI.Core.Application.Utility;

public class UserLinks(LinkGenerator linkGenerator, IDataShaper<UserDto> dataShaper) : IUserLinks
{
    private readonly LinkGenerator _linkGenerator = linkGenerator;
    private readonly IDataShaper<UserDto> _dataShaper = dataShaper;

    public LinkResponse TryGenerateLinks(IEnumerable<UserDto> usersDto, string fields, HttpContext httpContext)
    {
        var shapedUsers = ShapeData(usersDto, fields);

        if (ShouldGenerateLinks(httpContext))
            return ReturnLinkdedUsers(usersDto, fields, httpContext, shapedUsers);

        return ReturnShapedUsers(shapedUsers);
    }

    private LinkResponse ReturnShapedUsers(List<Entity> shapedUsers) =>
        new() { ShapedEntities = shapedUsers };

    private List<Entity> ShapeData(IEnumerable<UserDto> usersDto, string fields) =>
        _dataShaper.ShapeData(usersDto, fields)
            .Select(e => e.Entity)
            .ToList();

    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaType = (MediaTypeHeaderValue?) httpContext.Items["AcceptHeaderMediaType"];
        ArgumentNullException.ThrowIfNull(mediaType);
        return mediaType.SubTypeWithoutSuffix
            .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
    }
    private LinkResponse ReturnLinkdedUsers(IEnumerable<UserDto> usersDto, string fields, HttpContext httpContext, List<Entity> shapedUsers)
    {
        var UserDtoList = usersDto.ToList();

        for (var index = 0; index < UserDtoList.Count; index++)
        {
            var userLinks = CreateLinksForUsers(httpContext, UserDtoList[index].Id, fields);
            shapedUsers[index].Add("Links", userLinks);
        }

        var userCollection = new LinkCollectionWrapper<Entity>(shapedUsers);
        var linkedEmployees = CreateLinksForUsers(httpContext, userCollection);

        return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
    }

    private List<Link> CreateLinksForUsers(HttpContext httpContext, Guid id, string fields = "")
    {
        List<Link> links = [
            new Link(_linkGenerator.GetUriByAction(httpContext, "GetUser", values: new { id, fields })!, "self", "GET"),
            new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteUser", values: new { id })!, "delete_user", "DELETE"),
            new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateUser", values: new { id })!, "update_user", "PUT")
        ];

        return links;
    }
    private LinkCollectionWrapper<Entity> CreateLinksForUsers(HttpContext httpContext, LinkCollectionWrapper<Entity> employeesWrapper)
    {
        employeesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetUser", values: new { })!, "self", "GET"));
        return employeesWrapper;
    }
}
