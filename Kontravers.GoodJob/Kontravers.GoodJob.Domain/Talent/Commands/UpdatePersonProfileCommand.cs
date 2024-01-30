using System.Diagnostics;
using Kontravers.GoodJob.Domain.Messaging;

namespace Kontravers.GoodJob.Domain.Talent.Commands;

public class UpdatePersonProfileCommand : ICommand
{
    public UpdatePersonProfileCommand(DateTime createdUtc, string title, string description,
        string? skills, string personId, string profileId)
    {
        CreatedUtc = createdUtc;
        Title = title;
        Description = description;
        Skills = skills;
        PersonId = personId;
        ProfileId = profileId;
    }

    public string CommandName => _commandName;
    private const string _commandName = "UpdatePersonProfileCommand";
    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; } = new (_commandName);
    public DateTime CreatedUtc { get; }
    public string Title { get; }
    public string Description { get; }
    public string? Skills { get; }
    public string PersonId { get; }
    public string ProfileId { get; }
}