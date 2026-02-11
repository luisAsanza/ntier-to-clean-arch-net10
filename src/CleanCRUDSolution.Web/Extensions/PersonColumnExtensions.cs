using CleanCRUDSolution.Application.Features.Persons.Enums;

namespace CleanCRUDSolution.Web.Extensions
{    
    /// <summary>
    /// Provides extension methods for the <see cref="PersonColumn"/> enumeration.
    /// </summary>
    public static class PersonColumnExtensions
    {
        /// <summary>
        /// Converts a <see cref="PersonColumn"/> enum value to its corresponding user-friendly label.
        /// </summary>
        /// <param name="personColumn">The <see cref="PersonColumn"/> enum value to convert.</param>
        /// <returns>A string representing the human-readable label for the specified column.
        /// Returns "Unknown" if the enum value is not recognized.</returns>
        /// <example>
        /// <code>
        /// var label = PersonColumn.Name.ToLabel(); // Returns "Person Name"
        /// var emailLabel = PersonColumn.Email.ToLabel(); // Returns "Email"
        /// </code>
        /// </example>
        public static string ToLabel(this PersonColumn personColumn)
            => personColumn switch
            {
                PersonColumn.Name => "Person Name",
                PersonColumn.Email => "Email",
                PersonColumn.DateOfBirth => "Date Of Birth",
                PersonColumn.Age => "Age",
                PersonColumn.Gender => "Gender",
                PersonColumn.Country => "Country",
                PersonColumn.Address => "Address",
                PersonColumn.ReceiveNewsletter => "Receive Newsletter",
                _ => "Unknown"
            };
    }
}
