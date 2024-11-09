using AutoMapper;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Presentation;

public class MappingProfile : Profile
{
	private readonly UserManager<User> _userManager;
    public MappingProfile(UserManager<User> userManager)
	{
        _userManager = userManager;

        CreateMap<User, UserDto>()
			.ForMember(x => x.Roles, opt => 
				opt.MapFrom(x => GetRolesAsync(x))
			);
		CreateMap<UserForUpdateDto, User>();
		CreateMap<UserForRegistrationDto, User>();
	}

	private async Task<ICollection<string>> GetRolesAsync(User user)
	{
		return await _userManager.GetRolesAsync(user);
    }
}
