using System.Diagnostics;
using Kontravers.GoodJob.Domain.Messaging;

namespace Kontravers.GoodJob.Domain.Talent.Commands;

public class CreatePersonProfileCommand : ICommand
{
    public CreatePersonProfileCommand(string personId, string title, string description, string? skills,
        DateTime createdUtc)
    {
        PersonId = personId;
        Title = title;
        Description = description;
        Skills = skills;
        CreatedUtc = createdUtc;
    }

    public const string CommandName = "CreatePersonProfile";
    public string PersonId { get; }
    public string Title { get; }
    public string Description { get; }
    public string? Skills { get; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; } = new(CommandName);
    public DateTime CreatedUtc { get; }
}