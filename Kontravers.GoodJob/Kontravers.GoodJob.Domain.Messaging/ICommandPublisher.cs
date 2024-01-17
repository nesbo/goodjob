namespace Kontravers.GoodJob.Domain.Messaging;

public interface ICommandPublisher
{
    Task PublishAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : class, ICommand;
}