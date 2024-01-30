using System.Diagnostics;
using Kontravers.GoodJob.Domain.Messaging;

namespace Kontravers.GoodJob.Domain.Talent.Commands;

public class CreatePersonProfileCommand : ICommand
{
    private static string _commandName = "CreatePersonProfileCommand";

    public CreatePersonProfileCommand(string personId, string title, string description, string? skills,
        DateTime createdUtc)
    {
        PersonId = personId;
        Title = title;
        Description = description;
        Skills = skills;
        CreatedUtc = createdUtc;
    }

    public string CommandName => _commandName;
    public string PersonId { get; }
    public string Title { get; }
    public string Description { get; }
    public string? Skills { get; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; } = new(_commandName);
    public DateTime CreatedUtc { get; }
}