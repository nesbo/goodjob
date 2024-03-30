using System.Text.Json;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Messaging;

public class UserAccountConfirmedEventMapper : IAmAMessageMapper<UserAccountConfirmedEvent>,
    IAmAMessageMapperAsync<UserAccountConfirmedEvent>
{
    public Message MapToMessage(UserAccountConfirmedEvent request)
    {
        return new Message(
            new MessageHeader(request.Id, UserAccountConfirmedEvent.TopicName, MessageType.MT_EVENT,
                request.OccurredOn),
            new MessageBody(JsonSerializer.Serialize(request)));
    }

    public UserAccountConfirmedEvent MapToRequest(Message message)
    {
        return JsonSerializer.Deserialize<UserAccountConfirmedEvent>(message.Body.Value)!;
    }

    public Task<Message> MapToMessageAsync(UserAccountConfirmedEvent request,
        CancellationToken cancellationToken = new ())
    {
        return Task.FromResult(MapToMessage(request));
    }

    public Task<UserAccountConfirmedEvent> MapToRequestAsync(Message message,
        CancellationToken cancellationToken = new ())
    {
        return Task.FromResult(MapToRequest(message));
    }
}