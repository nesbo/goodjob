namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class PersonViewModel
{
    public PersonViewModel(Person person)
    {
        Id = person.Id.ToString();
        Name = person.Name;
        Email = person.Email;
        OrganisationId = person.OrganisationId.ToString();
        UpworkRssFeeds = person.UpworkRssFeeds
            .Select(f => new UpworkRssFeedListItemViewModel
            {
                Id = f.Id.ToString(),
                Title = f.Title
            })
            .ToArray();
        Profiles = person.Profiles
            .Select(p => new ProfileListItemViewModel
            {
                Id = p.Id.ToString(),
                Title = p.Title,
            })
            .ToArray();
    }
    
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string OrganisationId { get; set; }
    public UpworkRssFeedListItemViewModel[] UpworkRssFeeds { get; set; }
    public ProfileListItemViewModel[] Profiles { get; set; }
}