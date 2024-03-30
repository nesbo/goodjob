using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.Repositories;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class GetPerson
{
    private readonly IPersonQueryRepository _personQueryRepository;

    public GetPerson(IPersonQueryRepository personQueryRepository)
    {
        _personQueryRepository = personQueryRepository;
    }
    
    public async Task<PersonViewModel> GetAsync(GetPersonQuery query, CancellationToken cancellationToken)
    {
        var personId = int.Parse(query.PersonId);
        var person = await _personQueryRepository.GetAsync(personId, cancellationToken);
        if (person == null)
        {
            throw new NotFoundException($"Person with id {personId} not found");
        }

        return new PersonViewModel
        {
            Id = person.Id.ToString(),
            Name = person.Name,
            Email = person.Email,
            OrganisationId = person.OrganisationId.ToString(),
            UpworkRssFeeds = person.UpworkRssFeeds
                .Select(f => new UpworkRssFeedListItemViewModel
                {
                    Id = f.Id.ToString(),
                    Title = f.Title
                })
                .ToArray(),
            Profiles = person.Profiles
                .Select(p => new ProfileListItemViewModel
                {
                    Id = p.Id.ToString(),
                    Title = p.Title,
                })
                .ToArray()
        };
    }
}