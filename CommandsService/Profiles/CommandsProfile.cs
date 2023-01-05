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
    }
}