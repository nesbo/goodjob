namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class GetPersonByUserIdQuery
{
    public GetPersonByUserIdQuery(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }
}