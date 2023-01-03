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

    public PlatformsController(
        IRepository<Platform> repository,
        IMapper mapper,
        ICommandDataClient commandDataClient, ILogger<PlatformsController> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _logger = logger;
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

        try
        {
            await _commandDataClient.SendPlatformToCommand(platformDto);
        }
        catch (Exception e)
        {
            _logger.LogError("Could not send synchronously: {EMessage}", e.Message);
        }
        
        return CreatedAtAction("GetPlatform", new {id = platformDto.Id}, platformDto);
    }
}