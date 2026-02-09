namespace CleanCRUDSolution.Domain.Common
{
    /// <summary>
    /// Small guard helpers used throughout the domain to validate input values and throw domain-friendly exceptions.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Ensures a string is not null, empty, or whitespace. Returns trimmed value.
        /// </summary>
        public static string NotNullOrWhiteSpace(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value is required.", paramName);

            return value.Trim();
        }

        /// <summary>
        /// Allows nulls but disallows empty strings. Trims the value when not null.
        /// </summary>
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

        /// <summary>
        /// Validates that the supplied date is not in the future compared to the supplied 'today'.
        /// </summary>
        public static DateOnly? NullOrNotFutureDate(DateOnly? value, DateOnly today, string paramName)
        {
            if (value is null) return null;
            if (value > today) throw new ArgumentOutOfRangeException(paramName, value, "Date cannot be in the future.");
            return value;
        }

        /// <summary>
        /// Ensures a GUID is not empty.
        /// </summary>
        public static Guid NotEmpty(Guid value, string paramName)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Value cannot be empty.", paramName);
            return value;
        }
    }
}
