using System.Diagnostics;

namespace Kontravers.GoodJob.Domain.Messaging;

public class UserCreatedEvent : IDomainEvent
{
    public UserCreatedEvent(DateTime occurredOn)
    {
        OccurredOn = occurredOn;
    }
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; }
    public DateTime OccurredOn { get; set; }
    public const string TopicName = "UserCreatedEvent";
}