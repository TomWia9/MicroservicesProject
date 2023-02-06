using System.Text.Json;
using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Entities;
using CommandsService.Interfaces;

namespace CommandsService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;
    private readonly ILogger<EventProcessor> _logger;

    public EventProcessor(IServiceScopeFactory scopeFactory,
        IMapper mapper,
        ILogger<EventProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
        _logger = logger;
    }
    
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        if (eventType == EventType.PlatformPublished)
        {
            AddPlatform(message);
        }
    }

    private EventType DetermineEvent(string message)
    {
        _logger.LogInformation("Determining Event");

        var eventType = JsonSerializer.Deserialize<EventDto>(message);

        switch (eventType?.Event)
        {
            case "Platform_Published":
                _logger.LogInformation("Platform Published Event Detected");
                return EventType.PlatformPublished;
            default:
                _logger.LogInformation("Could not determine the event type");
                return EventType.Undetermined;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using var scope = _scopeFactory.CreateScope();
        var platformsRepository = scope.ServiceProvider.GetRequiredService<IRepository<Platform>>();
        var publishedPlatformDto = JsonSerializer.Deserialize<PublishedPlatformDto>(platformPublishedMessage);

        try
        {
            var platform = _mapper.Map<Platform>(publishedPlatformDto);

            if (!platformsRepository.Exists(x => x.ExternalId == platform.ExternalId))
            {
                platformsRepository.Create(platform);
                platformsRepository.SaveChanges();
                _logger.LogInformation("Platform added");
            }
            else
            {
                _logger.LogError("Platform already exists");
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Could not add Platform to database {Message}", e.Message);
        }
        
    }
}