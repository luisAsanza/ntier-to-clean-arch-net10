using CleanCRUDSolution.Application.Common.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanCRUDSolution.Infrastructure.Events
{
    public sealed class InProcessEventPublisher : IEventPublisher
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<InProcessEventPublisher> _logger;

        public InProcessEventPublisher(IServiceProvider sp, ILogger<InProcessEventPublisher> logger)
        {
            _sp = sp;
            _logger = logger;
        }

        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default) 
            where TEvent : IApplicationEvent
        {
            var handlers = _sp.GetServices<IEventHandler<TEvent>>().ToList();
            if (handlers.Count == 0) return;

            foreach (var handler in handlers)
            {
                try
                {
                    await handler.HandleAsync(@event, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, 
                        "Error handling event {EventType} with handler {HandlerType}", 
                        typeof(TEvent).Name, handler.GetType().Name);
                }
            }
        }
    }
}
