using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Talent.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Kontravers.GoodJob.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> ListAsync(
        [FromServices] ListPersonsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new ListPersonsQuery
        {
            Page = 1,
            PageSize = 10,
            OrganisationId = "1"
        };
        
        var result = await handler.ListAsync(query, cancellationToken);
        return Ok(result);
    }
}