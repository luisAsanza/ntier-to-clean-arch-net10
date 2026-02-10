using CleanCRUDSolution.Application.Common.Enums;
using CleanCRUDSolution.Application.Features.Persons.Enums;
using CleanCRUDSolution.Domain.Entities;
using CleanCRUDSolution.Domain.Enums;
using System.Globalization;

namespace CleanCRUDSolution.Infrastructure.Repositories
{
    public static class PersonRepositoryExtensions
    {
        public static IQueryable<Person> ApplySearch(this IQueryable<Person> query, PersonColumn? searchColumn, string? searchTerm, DateOnly today)
        {
            if(searchColumn is null || searchTerm is null)
            {
                return query;
            }

            searchTerm = searchTerm.Trim();

            if(searchTerm == string.Empty)
            {
                return query;
            }

            return searchColumn switch
            {
                PersonColumn.Name => query.Where(p => p.Name.Contains(searchTerm)),
                PersonColumn.Email => query.Where(p => p.Email != null && p.Email.Contains(searchTerm)),
                PersonColumn.DateOfBirth => ApplySearchDob(query, searchTerm),
                PersonColumn.Age => ApplySearchAge(query, searchTerm, today),
                PersonColumn.Gender => ApplySearchGender(query, searchTerm),
                PersonColumn.Country => query.Where(p => p.Country != null && p.Country.Name.Contains(searchTerm)),
                PersonColumn.Address => query.Where(p => p.Address != null && p.Address.Contains(searchTerm)),
                PersonColumn.ReceiveNewsletter => ApplySearchReceiveNewsLetter(query, searchTerm),
                _ => query
            };
        }

        public static IQueryable<Person> ApplySorting(this IQueryable<Person> query, PersonColumn? sortColumn, SortOrder? sortOrder)
        {
            if (sortColumn is null) return query;

            if (sortOrder is null) sortOrder = SortOrder.Asc;


            return (sortColumn, sortOrder) switch
            {
                (PersonColumn.Name, SortOrder.Asc) => query.OrderBy(p => p.Name),
                (PersonColumn.Name, SortOrder.Desc) => query.OrderByDescending(p => p.Name),

                (PersonColumn.Email, SortOrder.Asc) => query.OrderBy(p => p.Email),
                (PersonColumn.Email, SortOrder.Desc) => query.OrderByDescending(p => p.Email),

                (PersonColumn.DateOfBirth, SortOrder.Asc) => query.OrderBy(p => p.DateOfBirth),
                (PersonColumn.DateOfBirth, SortOrder.Desc) => query.OrderByDescending(p => p.DateOfBirth),

                (PersonColumn.Age, SortOrder.Asc) => query.OrderByDescending(p => p.DateOfBirth),
                (PersonColumn.Age, SortOrder.Desc) => query.OrderBy(p => p.DateOfBirth),

                (PersonColumn.Gender, SortOrder.Asc) => query.OrderBy(p => p.Gender),
                (PersonColumn.Gender, SortOrder.Desc) => query.OrderByDescending(p => p.Gender),

                (PersonColumn.Country, SortOrder.Asc) => query.OrderBy(p => p.Country != null ? p.Country.Name : string.Empty),
                (PersonColumn.Country, SortOrder.Desc) => query.OrderByDescending(p => p.Country != null ? p.Country.Name : string.Empty),

                (PersonColumn.Address, SortOrder.Asc) => query.OrderBy(p => p.Address),
                (PersonColumn.Address, SortOrder.Desc) => query.OrderByDescending(p => p.Address),

                (PersonColumn.ReceiveNewsletter, SortOrder.Asc) => query.OrderBy(p => p.ReceiveNewsLetters),
                (PersonColumn.ReceiveNewsletter, SortOrder.Desc) => query.OrderByDescending(p => p.ReceiveNewsLetters),

                _ => query.OrderBy(p => p.Name)
            };
        }


        private static IQueryable<Person> ApplySearchReceiveNewsLetter(IQueryable<Person> query, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return query;
            }

            bool includeTrue = "true".Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
            var includeFalse = "false".Contains(searchTerm, StringComparison.OrdinalIgnoreCase);

            return query.Where(p => p.ReceiveNewsLetters ? includeTrue : includeFalse);
        }

        private static IQueryable<Person> ApplySearchGender(IQueryable<Person> query, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return query;
            }

            var matchingGenders = Enum.GetValues<GenderOptions>()
                .Where(g => g.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matchingGenders.Count == 0) return query.Where(t => false);

            return query.Where(p => p.Gender.HasValue && matchingGenders.Contains(p.Gender.Value));
        }

        private static IQueryable<Person> ApplySearchDob(IQueryable<Person> query, string? searchTerm)
        {
            if(string.IsNullOrWhiteSpace(searchTerm))
            {
                return query;
            }

            if (DateOnly.TryParseExact(searchTerm, "dd MM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly dateOfBirthSearch))
            {
                return query.Where(p => p.DateOfBirth.HasValue && p.DateOfBirth.HasValue && p.DateOfBirth == dateOfBirthSearch);
            }
            
            return query.Where(t => false);
        }

        private static IQueryable<Person> ApplySearchAge(IQueryable<Person> query, string? searchTerm, DateOnly today)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return query;
            }

            if (int.TryParse(searchTerm, out int age))
            {
                var maxDob = today.AddYears(-age);
                var minDob = today.AddYears(-age - 1).AddDays(1);

               return query.Where(p => p.DateOfBirth.HasValue && p.DateOfBirth >= minDob && p.DateOfBirth <= maxDob);
            }

            return query.Where(p => false);
        }
    }
}
