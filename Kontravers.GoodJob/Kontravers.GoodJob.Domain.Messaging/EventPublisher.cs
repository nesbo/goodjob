using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Messaging;

public class EventPublisher(IAmACommandProcessor commandProcessor) : IEventPublisher
{
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) 
        where TEvent : class, IDomainEvent
    {
        return commandProcessor.PostAsync(@event, cancellationToken: cancellationToken);
    }
}