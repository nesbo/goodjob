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
    public string? Description { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    
    public static CreateJobCommand FromUpworkRssFeedItem(System.Xml.XmlNode xmlNode, DateTime createdUtc)
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
            Description = description,
            PublishedAtUtc = DateTime.Parse(pubDate!),
            CreatedUtc = createdUtc
        };
    }
}