using System.Text.Json;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Messaging;

public class UserCreatedEventMapper : IAmAMessageMapper<UserCreatedEvent>
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
}