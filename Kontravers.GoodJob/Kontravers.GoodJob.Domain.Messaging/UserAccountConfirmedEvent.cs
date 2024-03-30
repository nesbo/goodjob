using System.Diagnostics;

namespace Kontravers.GoodJob.Domain.Messaging;

public class UserAccountConfirmedEvent(DateTime occurredOn, string userId) : IDomainEvent
{
    public const string TopicName = "UserAccountConfirmedEvent";

    public Guid Id { get; set; } = Guid.NewGuid();
    public Activity Span { get; set; } = new(TopicName);
    
    public DateTime OccurredOn { get; set; } = occurredOn;
    public string UserId { get; set; } = userId;
}