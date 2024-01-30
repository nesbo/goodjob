using System.Diagnostics;
using System.Xml;
using Kontravers.GoodJob.Domain.Messaging;

namespace Kontravers.GoodJob.Domain.Work.Commands;

public class CreateJobCommand : ICommand
{
    public string CommandName => _commandName;
    private const string _commandName = "CreateJobCommand";
    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; } = new(_commandName);
    public DateTime CreatedUtc { get; set; }
    public DateTime PublishedAtUtc { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string Uuid { get; set; }
    public string PersonId { get; set; }
    public int? PreferredProfileId { get; set; }
    public JobSourceType JobSource { get; set; }
    public int PersonFeedId { get; set; }

    public static CreateJobCommand FromUpworkRssFeedItem(XmlNode xmlNode, DateTime createdUtc,
        string personId, int? preferredProfileId, int personFeedId)
    {
        var title = xmlNode.SelectSingleNode("title")?.InnerText;
        var link = xmlNode.SelectSingleNode("link")?.InnerText;
        var description = xmlNode.SelectSingleNode("description")?.InnerText;
        var pubDate = xmlNode.SelectSingleNode("pubDate")?.InnerText;
        var uuid = xmlNode.SelectSingleNode("guid")?.InnerText;

        return new CreateJobCommand
        {
            Title = title!,
            Url = link!,
            Description = description!,
            PublishedAtUtc = DateTime.Parse(pubDate!).ToUniversalTime(),
            CreatedUtc = createdUtc.ToUniversalTime(),
            Uuid = uuid!,
            PersonId = personId,
            PreferredProfileId = preferredProfileId,
            JobSource = JobSourceType.Upwork,
            PersonFeedId = personFeedId
        };
    }
}