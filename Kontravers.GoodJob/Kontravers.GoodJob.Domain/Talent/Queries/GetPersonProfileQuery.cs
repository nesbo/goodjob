namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class GetPersonProfileQuery
{
    public GetPersonProfileQuery(string personId, string profileId)
    {
        PersonId = personId;
        ProfileId = profileId;
    }

    public string PersonId { get; }
    public string ProfileId { get; }
}