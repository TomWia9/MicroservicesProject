using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Entities;

namespace PlatformService.Profiles;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        CreateMap<Platform, PlatformDto>();
        CreateMap<CreatePlatformDto, Platform>();
    }
}