namespace CleanCRUDSolution.Application.Common.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default) where TEvent : IApplicationEvent;
    }
}
