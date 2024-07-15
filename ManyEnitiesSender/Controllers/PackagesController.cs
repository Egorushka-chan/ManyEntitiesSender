using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManyEntitiesSender.Attributes;
using ManyEntitiesSender.DAL.Interfaces;
using Microsoft.Extensions.Options;
using ManyEntitiesSender.BLL.Settings;
using ManyEntitiesSender.BLL.Models.Requests;

namespace ManyEntitiesSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        [Cacheable]
        [HttpGet]
        public async Task<IActionResult> GetPackages()
        {
            return StatusCode(209);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("ensure")]
        public async Task<IActionResult> Ensure(IObjectGenerator generator)
        {
            await generator.EnsurePackageCount();
            return Ok();
        }
    }
}
