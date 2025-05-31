using LedgerLite.SharedKernel.Events;

namespace LedgerLite.Events.Tests.Unit;

public class PublisherTests
{
    private readonly IEventPublisher _eventPublisher = Substitute.For<IEventPublisher>();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();

    private Publisher _sut;

    public PublisherTests()
    {
        _sut = new Publisher(_eventPublisher, _serviceProvider);
    }

    [Fact]
    public async Task RegisterEventHandler()
    {
        ConfigureEventHandlers(Substitute.For<IEventHandler<ExampleEvent>>());
        var @event = new ExampleEvent("Hello!");

        var publishAction = () => _sut.PublishAsync(@event, CancellationToken.None);

        await publishAction.ShouldNotThrow();
    }

    [Fact]
    public async Task PublishEvents()
    {
        ConfigureEventHandlers(Substitute.For<IEventHandler<ExampleEvent>>());
        var @event = new ExampleEvent("Hello!");

        await _sut.PublishAsync(@event, CancellationToken.None);

        await _eventPublisher.Received(1).PublishAsync(
            Arg.Is<IEnumerable<EventExecutor>>(executors => executors.Count() == 1),
            @event,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task InvokeEventHandlers()
    {
        ConfigureRealEventPublisher();
        var handler = Substitute.For<IEventHandler<ExampleEvent>>();
        var @event = new ExampleEvent("Alright!");
        ConfigureEventHandlers(handler);

        await _sut.PublishAsync(@event, CancellationToken.None);

        await handler.Received(1).HandleAsync(@event, Arg.Any<CancellationToken>());
    }


    private void ConfigureRealEventPublisher()
    {
        _sut = new Publisher(new SequentialEventPublisher(), _serviceProvider);
    }

    private void ConfigureEventHandlers<TEvent>(params IList<IEventHandler<TEvent>> handlers) where TEvent : IEvent
    {
        var handlerType = typeof(IEnumerable<IEventHandler<TEvent>>);
        _serviceProvider.GetService(handlerType).Returns(handlers);
    }
}