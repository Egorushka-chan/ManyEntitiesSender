using ManyEntitiesSender.Attributes;
using ManyEntitiesSender.BLL.Models;
using ManyEntitiesSender.BLL.Models.Requests;
using ManyEntitiesSender.BLL.Services.Abstractions;
using ManyEntitiesSender.DAL.Entities;
using ManyEntitiesSender.DAL.Interfaces;
using ManyEntitiesSender.Models.Responses;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ManyEntitiesSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(200, Type = typeof(List<Body>))]
        [ProducesResponseType(200, Type = typeof(List<Hand>))]
        [ProducesResponseType(200, Type = typeof(List<Leg>))]
        [Cacheable]
        [HttpGet]
        public async Task<IResult> GetPackages([FromQuery] PackageRequest packageRequest, [FromServices] IPackageGetter getter)
        {
            int packageLimit = 2;
            int currentPackage = 1;

            var options = new EntityFilterOptions()
            {
                PropertyFilter = packageRequest.Filter,
                Skip = packageRequest.Skip,
                Take = packageRequest.Take
            };

            if(packageRequest.Table == "body")
            {
                List<Body> bodies = new List<Body>();
                await foreach(var package in getter.GetPackageAsync<Body>(options))
                {
                    bodies.AddRange(package);
                    if(currentPackage < packageLimit || packageLimit == -1)
                    {
                        currentPackage++;
                    }
                    else
                        break;
                }
                return TypedResults.Ok(bodies);
            }
            else if(packageRequest.Table == "hand")
            {
                List<Hand> hands = new List<Hand>();
                await foreach(var package in getter.GetPackageAsync<Hand>(options))
                {
                    hands.AddRange(package);
                    if(currentPackage < packageLimit || packageLimit == -1)
                    {
                        currentPackage++;
                    }
                    else
                        break;
                }
                return TypedResults.Ok(hands);
            }
            else if (packageRequest.Table == "leg")
            {
                List<Leg> legs = new List<Leg>();
                await foreach(var package in getter.GetPackageAsync<Leg>(options))
                {
                    legs.AddRange(package);
                    if(currentPackage < packageLimit || packageLimit == -1)
                    {
                        currentPackage++;
                    }
                    else
                        break;
                }
                return TypedResults.Ok(legs);
            }
            else
                return Results.BadRequest();
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
