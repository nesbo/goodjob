using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.Repositories;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class ListPersonsQueryHandler
{
    private readonly IPersonQueryRepository _personQueryRepository;

    public ListPersonsQueryHandler(IPersonQueryRepository personQueryRepository)
    {
        _personQueryRepository = personQueryRepository;
    }
    
    public async Task<PersonListViewModel> ListAsync(ListPersonsQuery query, CancellationToken cancellationToken)
    {
        var persons = await _personQueryRepository.ListAllAsync(cancellationToken);
        var result = persons.Select(p => new PersonListItemViewModel
        {
            Id = p.Id.ToString(),
            Name = p.Name,
            Email = p.Email,
            OrganisationId = p.OrganisationId.ToString(),
        }).ToArray();

        return new PersonListViewModel
        {
            Items = result,
            TotalCount = result.Length,
            Page = query.Page,
            PageSize = query.PageSize,
        };
    }
}