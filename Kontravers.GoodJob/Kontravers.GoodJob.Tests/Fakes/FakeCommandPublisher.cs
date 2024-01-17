using Kontravers.GoodJob.Domain.Messaging;

namespace Kontravers.GoodJob.Tests.Fakes;

public class FakeCommandPublisher : ICommandPublisher
{
    public List<ICommand> PublishedCommands { get; private set; } = new List<ICommand>();
    
    public Task PublishAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : class, ICommand
    {
        PublishedCommands.Add(command);
        return Task.CompletedTask;
    }
}