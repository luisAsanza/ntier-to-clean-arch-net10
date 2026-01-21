using CleanCRUDSolution.Domain.Common.Time;

namespace CleanCRUDSolution.Infrastructure.Time
{
    public class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public DateOnly GetToday(TimeZoneInfo timeZone)
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            return DateOnly.FromDateTime(localTime);
        }
    }
}
