using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Entities;
using CommandsService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly ILogger<PlatformsController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Platform> _platformsRepository;

    public PlatformsController(ILogger<PlatformsController> logger, IMapper mapper,
        IRepository<Platform> platformsRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _platformsRepository = platformsRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformDto>> GetPlatforms()
    {
        var platforms = _platformsRepository.GetAll();

        return Ok(_mapper.Map<IEnumerable<PlatformDto>>(platforms));
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        _logger.LogInformation("Inbound POST");
        return Ok("Inbound test of from Platforms Controller");
    }
}