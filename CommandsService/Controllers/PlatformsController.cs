using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly ILogger<PlatformsController> _logger;
    
    public PlatformsController(ILogger<PlatformsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        _logger.LogInformation("Inbound POST");
        return Ok("Inbound test of from Platforms Controller");
    }
}