using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.Repositories;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class GetPerson(IPersonQueryRepository personQueryRepository)
{
    public async Task<PersonViewModel> GetAsync(GetPersonQuery query, CancellationToken cancellationToken)
    {
        var personId = int.Parse(query.PersonId);
        var person = await personQueryRepository.GetAsync(personId, cancellationToken);
        if (person == null)
        {
            throw new NotFoundException($"Person with id {personId} not found");
        }

        return new PersonViewModel(person);
    }
    
    public async Task<PersonViewModel> GetAsync(GetPersonByUserIdQuery query, CancellationToken cancellationToken)
    {
        var person = await personQueryRepository.GetByUserId(query.UserId, cancellationToken);
        if (person == null)
        {
            throw new NotFoundException($"Person not found");
        }

        return new PersonViewModel(person);
    }
}