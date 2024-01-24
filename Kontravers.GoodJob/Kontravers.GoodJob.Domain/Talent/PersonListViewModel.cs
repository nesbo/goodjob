namespace Kontravers.GoodJob.Domain.Talent;

public class PersonListViewModel
{
    public PersonListItemViewModel[] Items { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}