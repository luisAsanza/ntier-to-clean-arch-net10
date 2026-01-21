namespace CleanCRUDSolution.Domain.Common.Time
{
    public interface IClock
    {
        DateTime UtcNow { get; }
        DateOnly GetToday(TimeZoneInfo timeZone);
    }
}
