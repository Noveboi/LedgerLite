using LedgerLite.SharedKernel.Events;

namespace LedgerLite.Events.Tests.Unit;

public class PublisherTests
{
    private readonly IEventPublisher _eventPublisher = Substitute.For<IEventPublisher>();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();

    private Publisher _sut;

    public PublisherTests()
    {
        _sut = new Publisher(eventPublisher: _eventPublisher, serviceProvider: _serviceProvider);
    }

    [Fact]
    public async Task RegisterEventHandler()
    {
        ConfigureEventHandlers(Substitute.For<IEventHandler<ExampleEvent>>());
        var @event = new ExampleEvent(Message: "Hello!");

        var publishAction = () => _sut.PublishAsync(e: @event, token: CancellationToken.None);

        await publishAction.ShouldNotThrow();
    }

    [Fact]
    public async Task PublishEvents()
    {
        ConfigureEventHandlers(Substitute.For<IEventHandler<ExampleEvent>>());
        var @event = new ExampleEvent(Message: "Hello!");

        await _sut.PublishAsync(e: @event, token: CancellationToken.None);

        await _eventPublisher.Received(requiredNumberOfCalls: 1).PublishAsync(
            Arg.Is<IEnumerable<EventExecutor>>(executors => executors.Count() == 1),
            e: @event,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task InvokeEventHandlers()
    {
        ConfigureRealEventPublisher();
        var handler = Substitute.For<IEventHandler<ExampleEvent>>();
        var @event = new ExampleEvent(Message: "Alright!");
        ConfigureEventHandlers(handler);

        await _sut.PublishAsync(e: @event, token: CancellationToken.None);

        await handler.Received(requiredNumberOfCalls: 1).HandleAsync(e: @event, Arg.Any<CancellationToken>());
    }


    private void ConfigureRealEventPublisher()
    {
        _sut = new Publisher(new SequentialEventPublisher(), serviceProvider: _serviceProvider);
    }

    private void ConfigureEventHandlers<TEvent>(params IList<IEventHandler<TEvent>> handlers) where TEvent : IEvent
    {
        var handlerType = typeof(IEnumerable<IEventHandler<TEvent>>);
        _serviceProvider.GetService(serviceType: handlerType).Returns(returnThis: handlers);
    }
}