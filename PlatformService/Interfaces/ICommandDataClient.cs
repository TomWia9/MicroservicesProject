using PlatformService.Dtos;

namespace PlatformService.Interfaces;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformDto platform);
}