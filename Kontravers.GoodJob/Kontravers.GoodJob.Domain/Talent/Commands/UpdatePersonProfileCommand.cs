using System.Diagnostics;
using Kontravers.GoodJob.Domain.Messaging;

namespace Kontravers.GoodJob.Domain.Talent.Commands;

public class UpdatePersonProfileCommand(
    DateTime createdUtc,
    string title,
    string description,
    string? skills,
    string userId,
    string profileId)
    : ICommand
{
    public string CommandName => _commandName;
    
    private const string _commandName = "UpdatePersonProfileCommand";
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; } = new (_commandName);
    public DateTime CreatedUtc { get; } = createdUtc;
    public string Title { get; } = title;
    public string Description { get; } = description;
    public string? Skills { get; } = skills;
    public string UserId { get; } = userId;
    public string ProfileId { get; } = profileId;
}