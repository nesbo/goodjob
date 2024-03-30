using Kontravers.GoodJob.Domain.Messaging;
using Kontravers.GoodJob.Domain.Talent.UseCases;
using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.MessagingGateway.RMQ;
using Paramore.Brighter.ServiceActivator;

namespace Kontravers.GoodJob.Worker;

public class BrighterRunner : BackgroundService
{
    private readonly ILogger<BrighterRunner> _logger;
    private readonly Dispatcher _dispatcher;

    private readonly Func<Type, IAmAMessageMapperAsync> _messageMapperFactoryAsync = type =>
        Activator.CreateInstance(type) as IAmAMessageMapperAsync;
    
    private readonly Func<Type, IAmAMessageTransform> _messageTransformFactory = type => new MessageTransform();
    private readonly Func<Type, IAmAMessageTransformAsync> _messageTransformFactoryAsync = type =>
        new MessageTransformAsync();

    private class MessageTransform : IAmAMessageTransform
    {
        public void Dispose() { }
        public void InitializeWrapFromAttributeParams(params object[] initializerList) { }
        public void InitializeUnwrapFromAttributeParams(params object[] initializerList) { }
        public Message Wrap(Message message) => message;
        public Message Unwrap(Message message) => message;
    }
    
    private class MessageTransformAsync : IAmAMessageTransformAsync
    {
        public void Dispose() { }
        public void InitializeWrapFromAttributeParams(params object[] initializerList) { }
        public void InitializeUnwrapFromAttributeParams(params object[] initializerList) { }
        public Task<Message> WrapAsync(Message message, CancellationToken cancellationToken = new()) =>
            Task.FromResult(message);
        public Task<Message> UnwrapAsync(Message message, CancellationToken cancellationToken = new()) =>
            Task.FromResult(message);
    }

    public BrighterRunner(
        IConfiguration configuration,
        ILogger<BrighterRunner> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        var subscriberRegistry = new SubscriberRegistry();
        subscriberRegistry.RegisterAsync<UserCreatedEvent, CreatePerson>();
        subscriberRegistry.RegisterAsync<UserAccountConfirmedEvent, EnablePerson>();

        var messageMapperRegistry = new MessageMapperRegistry(
            new ServiceProviderMapperFactory(serviceProvider),
            new SimpleMessageMapperFactoryAsync(_messageMapperFactoryAsync));
        messageMapperRegistry.Register<UserCreatedEvent, UserCreatedEventMapper>();
        messageMapperRegistry.RegisterAsync<UserCreatedEvent, UserCreatedEventMapper>();
        messageMapperRegistry.Register<UserAccountConfirmedEvent, UserAccountConfirmedEventMapper>();
        messageMapperRegistry.RegisterAsync<UserAccountConfirmedEvent, UserAccountConfirmedEventMapper>();

        var messageTransformerFactory = new SimpleMessageTransformerFactory(_messageTransformFactory);
        var messageTransformerFactoryAsync = new SimpleMessageTransformerFactoryAsync(_messageTransformFactoryAsync);
        
        var amqpUriSpecification = new AmqpUriSpecification(
            new Uri(configuration.GetConnectionString("RabbitMq")!));

        var rmqIdentityEventsConsumerConnection = new RmqMessagingGatewayConnection
        {
            AmpqUri = amqpUriSpecification,
            Name = Constants.IdentityEventsExchange,
            PersistMessages = true,
            Exchange = new Exchange(Constants.IdentityEventsExchange, type: "topic", durable: true),
            DeadLetterExchange = new Exchange($"{Constants.IdentityEventsExchange}.Dlq",
                durable: true)
        };
        var rmqMessageConsumerFactory = new RmqMessageConsumerFactory(rmqIdentityEventsConsumerConnection);

        var builder = DispatchBuilder.With()
            .CommandProcessorFactory(() =>
                new CommandProcessorProvider(
                    CommandProcessorBuilder.With()
                        .Handlers(new HandlerConfiguration(
                            subscriberRegistry: subscriberRegistry,
                            handlerFactory: new ServiceProviderHandlerFactory(serviceProvider)))
                        .Policies(new DefaultPolicy())
                        .NoExternalBus()
                        .RequestContextFactory(new InMemoryRequestContextFactory())
                        .Build())
            )
            .MessageMappers(messageMapperRegistry, messageMapperRegistry,
                messageTransformerFactory, messageTransformerFactoryAsync)
            .DefaultChannelFactory(new ChannelFactory(rmqMessageConsumerFactory))
            .Subscriptions(new Subscription[]
            {
                new RmqSubscription<UserCreatedEvent>(
                    name: new SubscriptionName(UserCreatedEvent.TopicName),
                    channelName: new ChannelName(UserCreatedEvent.TopicName),
                    routingKey: new RoutingKey(UserCreatedEvent.TopicName),
                    noOfPerformers: 1,
                    runAsync: true),
                new RmqSubscription<UserAccountConfirmedEvent>(
                    name: new SubscriptionName(UserAccountConfirmedEvent.TopicName),
                    channelName: new ChannelName(UserAccountConfirmedEvent.TopicName),
                    routingKey: new RoutingKey(UserAccountConfirmedEvent.TopicName),
                    noOfPerformers: 1,
                    runAsync: true)
            });
        
        _dispatcher = builder.Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogTrace("Starting consumers...");
        _dispatcher.Receive();
        _logger.LogInformation("[{ConsumersCount}] consumers started. Dispatcher state [{DispatcherState}] ",
            _dispatcher.Consumers.Count(), _dispatcher.State);
        
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace("Stopping consumers...");
        await _dispatcher.End();
        _logger.LogInformation("Consumers stopped");
    }
}