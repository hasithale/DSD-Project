using Microsoft.AspNetCore.Mvc;

namespace DSD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { online = true });
    }
}
