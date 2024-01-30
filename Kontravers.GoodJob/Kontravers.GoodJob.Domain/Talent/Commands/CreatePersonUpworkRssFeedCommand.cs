using System.Diagnostics;
using Kontravers.GoodJob.Domain.Messaging;

namespace Kontravers.GoodJob.Domain.Talent.Commands;

public class CreatePersonUpworkRssFeedCommand : ICommand
{
    public CreatePersonUpworkRssFeedCommand(DateTime createdUtc, int personId, string rootUrl, string relativeUrl,
        bool autoSendEmailEnabled, bool autoGenerateProposalsEnabled, int? preferredProfileId,
        byte minimumFetchIntervalInMinutes, string title)
    {
        CreatedUtc = createdUtc;
        PersonId = personId;
        RootUrl = rootUrl;
        RelativeUrl = relativeUrl;
        AutoSendEmailEnabled = autoSendEmailEnabled;
        AutoGenerateProposalsEnabled = autoGenerateProposalsEnabled;
        PreferredProfileId = preferredProfileId;
        MinimumFetchIntervalInMinutes = minimumFetchIntervalInMinutes;
        Title = title;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; } = new(_commandName);
    public string CommandName => _commandName;
    private const string _commandName = "CreatePersonUpworkRssFeedCommand";
    public DateTime CreatedUtc { get; }
    public string RootUrl { get; }
    public string RelativeUrl { get; }
    public bool AutoSendEmailEnabled { get; }
    public bool AutoGenerateProposalsEnabled { get; }
    public int? PreferredProfileId { get; }
    public byte MinimumFetchIntervalInMinutes { get; }
    public string Title { get; }
    public int PersonId { get; }
}