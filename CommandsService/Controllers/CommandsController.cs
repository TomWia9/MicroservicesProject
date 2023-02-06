using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Entities;
using CommandsService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[ApiController]
[Route("api/c/platforms/{platformId:int}/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly ILogger<CommandsController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Command> _commandsRepository;
    private readonly IRepository<Platform> _platformsRepository;

    public CommandsController(ILogger<CommandsController> logger, IMapper mapper,
        IRepository<Command> commandsRepository, IRepository<Platform> platformsRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _commandsRepository = commandsRepository;
        _platformsRepository = platformsRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandDto>> GetPlatformCommands(int platformId)
    {
        if (!_platformsRepository.Exists(platformId))
        {
            return NotFound();
        }
        
        var commands = _commandsRepository.GetAll(x => x.PlatformId == platformId);

        return Ok(_mapper.Map<IEnumerable<CommandDto>>(commands));
    }
    
    [HttpGet("{commandId:int}")]
    public ActionResult<CommandDto> GetCommand(int platformId, int commandId)
    {
        if (!_platformsRepository.Exists(platformId))
        {
            return NotFound();
        }
        
        var command = _commandsRepository.GetById(commandId);

        if (command == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CommandDto>(command));
    }

    [HttpPost]
    public ActionResult<CommandDto> CreateCommand(int platformId, CreateCommandDto command)
    {
        if (!_platformsRepository.Exists(platformId))
        {
            return NotFound();
        }

        command.PlatformId = platformId;

        var commandEntity = _mapper.Map<Command>(command);
        
        _commandsRepository.Create(commandEntity);
        _commandsRepository.SaveChanges();

        var commandDto = _mapper.Map<CommandDto>(commandEntity);
        
        return CreatedAtAction("GetCommand", new { platformId, commandId = commandDto.Id}, commandDto);
    }
}