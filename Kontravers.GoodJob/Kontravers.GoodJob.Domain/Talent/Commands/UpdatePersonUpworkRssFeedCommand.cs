using System.Diagnostics;
using Kontravers.GoodJob.Domain.Messaging;

namespace Kontravers.GoodJob.Domain.Talent.Commands;

public class UpdatePersonUpworkRssFeedCommand : ICommand
{
    public UpdatePersonUpworkRssFeedCommand(DateTime createdUtc, int personId, int personFeedId,
        string rootUrl, string relativeUrl, bool autoSendEmailEnabled, bool autoGenerateProposalsEnabled,
        int? preferredProfileId, byte minFetchIntervalInMinutes, string title)
    {
        CreatedUtc = createdUtc;
        PersonId = personId;
        PersonFeedId = personFeedId;
        RootUrl = rootUrl;
        RelativeUrl = relativeUrl;
        AutoSendEmailEnabled = autoSendEmailEnabled;
        AutoGenerateProposalsEnabled = autoGenerateProposalsEnabled;
        PreferredProfileId = preferredProfileId;
        MinFetchIntervalInMinutes = minFetchIntervalInMinutes;
        Title = title;
    }

    public static string CommandName { get; } = "UpdatePersonUpworkRssFeedCommand";
    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; } = new (CommandName);
    public DateTime CreatedUtc { get; }
    public int PersonId { get; }
    public int PersonFeedId { get; }
    public string RootUrl { get; }
    public string RelativeUrl { get; }
    public bool AutoSendEmailEnabled { get; }
    public bool AutoGenerateProposalsEnabled { get; }
    public int? PreferredProfileId { get; }
    public byte MinFetchIntervalInMinutes { get; }
    public string Title { get; }
}