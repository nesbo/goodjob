using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Messaging;

public interface ICommand : IRequest
{
    string CommandName { get; }
    DateTime CreatedUtc { get; }
}