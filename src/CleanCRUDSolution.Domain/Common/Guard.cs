namespace CleanCRUDSolution.Domain.Common
{
    public static class Guard
    {
        public static string NotNullOrWhiteSpace(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value is required.", paramName);

            return value.Trim();
        }

        public static string? NullOrNotWhiteSpace(string? value, string paramName)
        {
            if(value is null)
            {
                return value;
            }

            value = value.Trim();

            if (value.Length == 0)
                throw new ArgumentException("Value cannot be empty.", paramName);

            return value;
        }

        public static DateOnly? NullOrNotFutureDate(DateOnly? value, DateOnly today, string paramName)
        {
            if (value is null) return null;
            if (value > today) throw new ArgumentOutOfRangeException(paramName, value, "Date cannot be in the future.");
            return value;
        }

        public static Guid NotEmpty(Guid value, string paramName)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Value cannot be empty.", paramName);
            return value;
        }
    }
}
