using System.Diagnostics;

namespace Kontravers.GoodJob.Messaging.Commands;

public class CreateJobCommand : ICommand
{
    public static readonly string CommandName  = "CreateJobCommand";
    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; } = new (CommandName);
    public DateTime CreatedUtc { get; set; }
}