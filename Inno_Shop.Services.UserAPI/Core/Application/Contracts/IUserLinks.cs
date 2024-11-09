using Inno_Shop.Services.UserAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;

namespace Inno_Shop.Services.UserAPI.Core.Application.Contracts;

public interface IUserLinks
{
    LinkResponse TryGenerateLinks(
        IEnumerable<UserDto> usersDto, 
        string fields, 
        HttpContext httpContext);
  
}
