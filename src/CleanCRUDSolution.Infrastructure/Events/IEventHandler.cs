using CleanCRUDSolution.Application.Common.Events;

namespace CleanCRUDSolution.Infrastructure.Events
{
    public interface IEventHandler<TEvent> where TEvent : IApplicationEvent
    {
        Task HandleAsync(TEvent appEvent, CancellationToken ct);
    }
}
