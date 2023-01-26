using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.Entities;
using PlatformService.Interfaces;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IRepository<Platform> _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly ILogger<PlatformsController> _logger;
    private readonly IMessageBusClient<PublishedPlatformDto> _messageBusClient;

    public PlatformsController(
        IRepository<Platform> repository,
        IMapper mapper,
        ICommandDataClient commandDataClient,
        ILogger<PlatformsController> logger,
        IMessageBusClient<PublishedPlatformDto> messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _logger = logger;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformDto>> GetPlatforms()
    {
        var platforms = _repository.GetAll();

        return Ok(_mapper.Map<IEnumerable<PlatformDto>>(platforms));
    }
    
    [HttpGet("{id:int}")]
    public ActionResult<IEnumerable<PlatformDto>> GetPlatform(int id)
    {
        var platform = _repository.GetById(id);

        if (platform == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PlatformDto>(platform));
    }
    
    [HttpPost]
    public async Task<ActionResult<PlatformDto>> CreatePlatform([FromBody] CreatePlatformDto createPlatform)
    {
        var platformEntity = _mapper.Map<Platform>(createPlatform);
        
        _repository.Create(platformEntity);
        _repository.SaveChanges();

        var platformDto = _mapper.Map<PlatformDto>(platformEntity);

        //Send sync message
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformDto);
        }
        catch (Exception e)
        {
            _logger.LogError("Could not send synchronously: {EMessage}", e.Message);
        }
        
        //Send async message
        try
        {
            var publishedPlatformDto = _mapper.Map<PublishedPlatformDto>(platformDto);
            publishedPlatformDto.Event = "Platform_Published";
            _messageBusClient.Publish(publishedPlatformDto);
        }
        catch (Exception e)
        {
            _logger.LogError("Could not send asynchronously: {EMessage}", e.Message);
        }
        
        return CreatedAtAction("GetPlatform", new {id = platformDto.Id}, platformDto);
    }
}