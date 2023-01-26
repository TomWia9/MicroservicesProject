using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Entities;

namespace CommandsService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformDto>();
        CreateMap<Command, CommandDto>();
        CreateMap<CreateCommandDto, Command>();
        CreateMap<PublishedPlatformDto, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
    }
}