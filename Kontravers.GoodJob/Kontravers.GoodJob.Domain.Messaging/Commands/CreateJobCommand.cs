using System.Diagnostics;
using System.Xml;

namespace Kontravers.GoodJob.Domain.Messaging.Commands;

public class CreateJobCommand : ICommand
{
    public static readonly string CommandName  = "CreateJobCommand";
    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; } = new (CommandName);
    public DateTime CreatedUtc { get; set; }
    public DateTime PublishedAtUtc { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string Uuid { get; set; }
    public string PersonId { get; set; }
    public int? PreferredPortfolioId { get; set; }
    public JobSourceCommandType JobSource { get; set; }
    public int PersonFeedId { get; set; }

    public static CreateJobCommand FromUpworkRssFeedItem(XmlNode xmlNode, DateTime createdUtc,
        string personId, int? preferredPortfolioId, int personFeedId)
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
            PublishedAtUtc = DateTime.Parse(pubDate!).ToLocalTime().ToUniversalTime(),
            CreatedUtc = createdUtc.ToUniversalTime(),
            Uuid = uuid!,
            PersonId = personId,
            PreferredPortfolioId = preferredPortfolioId,
            JobSource = JobSourceCommandType.Upwork,
            PersonFeedId = personFeedId
        };
    }
}