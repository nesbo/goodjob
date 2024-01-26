namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class GetPersonQuery
{
    public GetPersonQuery(string personId)
    {
        PersonId = personId;
    }
    public string PersonId { get; set; }
}