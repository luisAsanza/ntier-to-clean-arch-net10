using CleanCRUDSolution.Domain.Common;
using CleanCRUDSolution.Domain.Entities.Base;
using CleanCRUDSolution.Domain.Enums;
using CleanCRUDSolution.Domain.Exceptions;

namespace CleanCRUDSolution.Domain.Entities
{
    public class Person : Entity
    {
        public string Name { get; private set; }
        public string? Email { get; private set; }
        public DateOnly? DateOfBirth { get; private set; }
        public GenderOptions? Gender { get; private set; }
        public Guid? CountryId { get; private set; }
        public string? Address { get; private set; }
        public bool ReceiveNewsLetters { get; private set; }
        public string? TIN { get; private set; }

        //Navigation Property is acceptable on Clean Architecture .NET projects.
        //Private set avoid developers to change it. EFCore will handle it.
        public Country? Country { get; private set; }

        public Person(string name) : base(Guid.NewGuid())
        {
            Name = Guard.NotNullOrWhiteSpace(name, nameof(Name));
            ReceiveNewsLetters = false;
        }

        public void UpdateEmail(string? email)
        {
            Email = Guard.NullOrNotWhiteSpace(email, nameof(Email));
        }

        public void UpdateDateOfBirth(DateOnly? dateOfBirth, DateOnly today)
        {
            DateOfBirth = Guard.NullOrNotFutureDate(dateOfBirth, today, nameof(DateOfBirth));
        }

        public void UpdateGender(GenderOptions? gender)
        {
            Gender = gender;
        }

        public void MoveTo(Guid countryId, string newAddress)
        {
            CountryId = Guard.NotEmpty(countryId, nameof(CountryId));
            Address = Guard.NotNullOrWhiteSpace(newAddress, nameof(newAddress));
            TIN = null; // Reset TIN when moving to a new country
        }

        public void UpdateReceiveNewsLetters(bool receiveNewsLetters)
        {
            ReceiveNewsLetters = receiveNewsLetters;
        }

        public void UpdateTin(string? tin)
        {
            tin = Guard.NullOrNotWhiteSpace(tin, nameof(TIN));

            if(tin is not null && tin.Length < 5)
                throw new InvalidTinFormatException(tin);

            TIN = tin;
        }
    }
}
