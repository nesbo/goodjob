using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Messaging;

public interface IDomainEvent : IRequest
{
    public DateTime OccurredOn { get; set; }
}