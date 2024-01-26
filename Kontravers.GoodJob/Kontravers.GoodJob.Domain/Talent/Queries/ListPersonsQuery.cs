namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class ListPersonsQuery
{
    public ListPersonsQuery(string organisationId, int page, int pageSize)
    {
        OrganisationId = organisationId;
        Page = page;
        PageSize = pageSize;
    }

    public string OrganisationId { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}