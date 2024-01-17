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
        return _commandProcessor.PostAsync(command, cancellationToken: cancellationToken);
    }
}