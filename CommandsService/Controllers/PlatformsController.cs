using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformsController : ControllerBase
{
    public PlatformsController()
    {
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        return Ok("Inbound test of from Platforms Controller");
    }
}