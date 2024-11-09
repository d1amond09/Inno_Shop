using AutoMapper;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Presentation;

public class MappingProfile : Profile
{
    public MappingProfile()
	{
		CreateMap<User, UserDto>();
		CreateMap<UserForUpdateDto, User>();
		CreateMap<UserForRegistrationDto, User>();
	}
}
