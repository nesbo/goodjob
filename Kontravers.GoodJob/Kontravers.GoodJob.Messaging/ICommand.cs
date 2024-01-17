using Paramore.Brighter;

namespace Kontravers.GoodJob.Messaging;

public interface ICommand : IRequest
{
    static string CommandName { get; }
    DateTime CreatedUtc { get; }
}