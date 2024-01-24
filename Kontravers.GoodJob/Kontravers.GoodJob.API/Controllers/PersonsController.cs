using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Kontravers.GoodJob.API.Controllers;

[ApiController]
[Route("api/persons")]
public class PersonsController : Controller
{
    [HttpGet]
    [ProducesResponseType(typeof(PersonListViewModel), StatusCodes.Status200OK)]
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
    
    [HttpGet("{personId}")]
    [ProducesResponseType(typeof(PersonViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync(
        [FromServices] GetPersonQueryHandler handler,
        string personId,
        CancellationToken cancellationToken)
    {
        var query = new GetPersonQuery
        {
            PersonId = personId,
        };
        
        try
        {
            var result = await handler.GetAsync(query, cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}