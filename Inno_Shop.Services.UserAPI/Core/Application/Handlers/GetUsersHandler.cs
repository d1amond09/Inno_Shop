using AutoMapper;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Application.Contracts;
using Inno_Shop.Services.UserAPI.Core.Application.Queries;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Core.Domain.RequestFeatures;
using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using Inno_Shop.Services.UserAPI.Infastructure.Persistence.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop.Services.UserAPI.Core.Application.Handlers;

public class GetUsersHandler
    (UserManager<User> userManager, IMapper mapper, IUserLinks userLinks) :
    IRequestHandler<GetUsersQuery, ApiBaseResponse>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IMapper _mapper = mapper;
    private readonly IUserLinks _userLinks = userLinks;

    public async Task<ApiBaseResponse> Handle(
        GetUsersQuery request, 
        CancellationToken cancellationToken)
    {
        var users = _userManager.Users
            .Search(request.LinkParameters.UserParameters.SearchTerm)    
            .Sort(request.LinkParameters.UserParameters.OrderBy);
            
        var count = await users.CountAsync(cancellationToken);
            
        var usersToReturn = await users
            .Skip((request.LinkParameters.UserParameters.PageNumber - 1) *
                   request.LinkParameters.UserParameters.PageSize)
            .Take(request.LinkParameters.UserParameters.PageSize)
            .ToListAsync(cancellationToken);
        
        var usersWithMetaData = new PagedList<User>(
            usersToReturn, count,
            request.LinkParameters.UserParameters.PageNumber,
            request.LinkParameters.UserParameters.PageSize
        );

        var usersDto = _mapper.Map<IEnumerable<UserDto>>(usersWithMetaData);

        for(int i = 0; i < usersDto.Count(); i++)
        {
            usersDto.ElementAt(i).Roles = await _userManager.GetRolesAsync(usersToReturn[i]);
        }

        var links = _userLinks.TryGenerateLinks(
            usersDto,
            request.LinkParameters.UserParameters.Fields,
            request.LinkParameters.Context
        );

        (LinkResponse, MetaData) result = new(links, usersWithMetaData.MetaData);
        return new ApiOkResponse<(LinkResponse, MetaData)>(result);
    }
}

