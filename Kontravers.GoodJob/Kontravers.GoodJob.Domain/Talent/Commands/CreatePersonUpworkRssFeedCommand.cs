using System.Diagnostics;
using Kontravers.GoodJob.Domain.Messaging;

namespace Kontravers.GoodJob.Domain.Talent.Commands;

public class CreatePersonUpworkRssFeedCommand(
    DateTime createdUtc,
    string userId,
    string rootUrl,
    string relativeUrl,
    bool autoSendEmailEnabled,
    bool autoGenerateProposalsEnabled,
    int? preferredProfileId,
    byte minimumFetchIntervalInMinutes,
    string title)
    : ICommand
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; } = new(_commandName);
    public string CommandName => _commandName;
    private const string _commandName = "CreatePersonUpworkRssFeedCommand";
    public DateTime CreatedUtc { get; } = createdUtc;
    public string RootUrl { get; } = rootUrl;
    public string RelativeUrl { get; } = relativeUrl;
    public bool AutoSendEmailEnabled { get; } = autoSendEmailEnabled;
    public bool AutoGenerateProposalsEnabled { get; } = autoGenerateProposalsEnabled;
    public int? PreferredProfileId { get; } = preferredProfileId;
    public byte MinimumFetchIntervalInMinutes { get; } = minimumFetchIntervalInMinutes;
    public string Title { get; } = title;
    public string UserId { get; } = userId;
}