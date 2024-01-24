namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class PersonViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string OrganisationId { get; set; }
    public UpworkRssFeedListItemViewModel[] UpworkRssFeeds { get; set; }
    public ProfileListItemViewModel[] Profiles { get; set; }
}