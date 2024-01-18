using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Messaging;

public class CommandPublisher : ICommandPublisher
{
    private readonly IAmACommandProcessor _commandProcessor;

    public CommandPublisher(IAmACommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }
    
    public Task PublishAsync<TCommand>(TCommand command, CancellationToken cancellationToken) 
        where TCommand : class, ICommand
    {
        // let's go with send at first, don't publish to bus
        return _commandProcessor.SendAsync(command, cancellationToken: cancellationToken);
    }
}