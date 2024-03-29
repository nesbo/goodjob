using System.Diagnostics;

namespace Kontravers.GoodJob.Domain.Messaging;

public class UserCreatedEvent : IDomainEvent
{
    public const string TopicName = "UserCreatedEvent";
    
    public UserCreatedEvent(DateTime occurredOn, string name,
        string email, string organizationId, string userId)
    {
        OccurredOn = occurredOn;
        Name = name;
        Email = email;
        OrganizationId = organizationId;
        UserId = userId;
    }
    
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Activity Span { get; set; }
    public DateTime OccurredOn { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string OrganizationId { get; set; }
    public string UserId { get; set; }
}