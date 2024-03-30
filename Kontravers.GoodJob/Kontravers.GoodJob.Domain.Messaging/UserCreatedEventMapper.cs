using System.Text.Json;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Messaging;

public class UserCreatedEventMapper : IAmAMessageMapper<UserCreatedEvent>, IAmAMessageMapperAsync<UserCreatedEvent>
{
    public Message MapToMessage(UserCreatedEvent request)
    {
        return new Message(
            new MessageHeader(request.Id, UserCreatedEvent.TopicName, MessageType.MT_EVENT,
                request.OccurredOn),
            new MessageBody(JsonSerializer.Serialize(request)));
    }

    public UserCreatedEvent MapToRequest(Message message)
    {
        return JsonSerializer.Deserialize<UserCreatedEvent>(message.Body.Value)!;
    }

    public Task<Message> MapToMessageAsync(UserCreatedEvent request, CancellationToken cancellationToken = new ())
    {
        return Task.FromResult(MapToMessage(request));
    }

    public Task<UserCreatedEvent> MapToRequestAsync(Message message, CancellationToken cancellationToken = new ())
    {
        return Task.FromResult(MapToRequest(message));
    }
}