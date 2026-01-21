using CleanCRUDSolution.Application.Features.Persons.DTOs;
using CleanCRUDSolution.Domain.Entities;

namespace CleanCRUDSolution.Application.Features.Persons.Mappers
{
    public static class PersonMappingExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse(
                person.Id,
                person.Name,
                person.Email,
                person.DateOfBirth,
                person.Gender?.ToString(),
                person.CountryId,
                person.Country?.Name,
                person.Address,
                person.ReceiveNewsLetters,
                CalculateAge(person.DateOfBirth),
                person.TIN);
        }

        public static Person ToPerson(this PersonAddRequest request, DateOnly today)
        {
            var person = new Person(request.Name);

            person.UpdateEmail(request.Email);
            person.UpdateDateOfBirth(request.DateOfBirth, today);
            person.UpdateGender(request.Gender);
            person.UpdateReceiveNewsLetters(request.ReceiveNewsLetters);
            person.UpdateTin(request.TIN);

            if(request.CountryId.HasValue && !string.IsNullOrWhiteSpace(request.Address))
            {
                person.MoveTo(request.CountryId.Value, request.Address);
            }

            return person;
        }

        public static void UpdateFromRequest(this Person person, PersonUpdateRequest request, DateOnly today)
        {
            person.UpdateEmail(request.Email);
            person.UpdateDateOfBirth(request.DateOfBirth, today);
            person.UpdateGender(request.Gender);
            person.UpdateReceiveNewsLetters(request.ReceiveNewsLetters);
            person.UpdateTin(request.TIN);

            if (request.CountryId.HasValue && !string.IsNullOrWhiteSpace(request.Address))
            {
                person.MoveTo(request.CountryId.Value, request.Address);
            }
        }

        private static double? CalculateAge(DateOnly? dateOfBirth)
        {
            if (dateOfBirth == null) return null;

            var dob = dateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
            var days = (DateTime.UtcNow - dob).TotalDays;

            //Calculate age in years with decimal places
            return Math.Round(days / 365.25, 2);
        }
    }
}
