using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManyEntitiesSender.Attributes;
using ManyEntitiesSender.DAL.Interfaces;
using Microsoft.Extensions.Options;
using ManyEntitiesSender.BLL.Settings;
using ManyEntitiesSender.BLL.Models.Requests;
using ManyEntitiesSender.Models.Responses;

namespace ManyEntitiesSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(201)] // если прошло через middleware
        [Cacheable]
        [HttpGet]
        public IResult GetPackages([FromQuery] PackageRequest packageRequest)
        {
            ErrorResponse errorResponse = new()
            {
                Body = "Middleware passed request to controller. This means that request wasn't handled properly"
            };
            return TypedResults.BadRequest(errorResponse);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("ensure")]
        public async Task<IActionResult> Ensure(IObjectGenerator generator)
        {
            await generator.EnsurePartsCount();
            return Ok();
        }
    }
}
