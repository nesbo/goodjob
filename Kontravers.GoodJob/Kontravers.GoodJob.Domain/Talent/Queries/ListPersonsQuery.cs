namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class ListPersonsQuery
{
    public string OrganisationId { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}