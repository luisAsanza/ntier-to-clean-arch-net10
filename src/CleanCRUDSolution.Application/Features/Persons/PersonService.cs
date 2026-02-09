using CleanCRUDSolution.Application.Abstractions;
using CleanCRUDSolution.Application.Common.Mappers;
using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Persons.DTOs;
using CleanCRUDSolution.Application.Features.Persons.Mappers;
using CleanCRUDSolution.Domain.Common.Time;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CleanCRUDSolution.Application.Features.Persons
{
    /// <summary>
    /// Service to perform CRUD operations and queries related to Person entities.
    /// </summary>
    public partial class PersonService : IPersonService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly IClock _clock;
        private readonly ILogger<PersonService> _logger;
        private readonly IValidator<PersonAddRequest> _personAddRequestValidator;
        private readonly IValidator<PersonUpdateRequest> _personUpdateRequestValidator;
        private readonly IValidator<GetPersonRequest> _getPersonRequestValidator;
        public PersonService(IPersonsRepository personsRepository,
            IClock clock,
            ILogger<PersonService> logger,
            IValidator<PersonAddRequest> personAddRequestValidator, 
            IValidator<PersonUpdateRequest> personUpdateRequestValidator,
            IValidator<GetPersonRequest> getPersonRequestValidator)
        {
            _personsRepository = personsRepository;
            _clock = clock;
            _logger = logger;
            _personAddRequestValidator = personAddRequestValidator;
            _personUpdateRequestValidator = personUpdateRequestValidator;
            _getPersonRequestValidator = getPersonRequestValidator;
        }

        public async Task<Result<PersonResponse>> AddPersonAsync(PersonAddRequest request)
        {
            var validationResult = await _personAddRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) return Result<PersonResponse>.Failure(validationResult.ToErrorList());

            // Map and persist
            var person = request.ToPerson(_clock.GetToday(TimeZoneInfo.Local));
            var addedPerson = await _personsRepository.AddAsync(person);
            _logger.LogInformation("Person added with ID: {PersonId}", addedPerson.Id);

            return Result<PersonResponse>.Success(addedPerson.ToPersonResponse());
        }

        public async Task<Result> UpdatePersonAsync(PersonUpdateRequest request)
        {
            var validationResult = await _personUpdateRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) return Result<PersonResponse>.Failure(validationResult.ToErrorList());

            // Get and validate it exists
            var person = await _personsRepository.GetByIdAsync(request.Id);
            if(person is null) {
                return Result.Failure(PersonErrors.NotFound(request.Id));
            }

            // Update and persist
            person.UpdateFromRequest(request, _clock.GetToday(TimeZoneInfo.Local));
            await _personsRepository.UpdateAsync(person);
            _logger.LogInformation("Person updated with ID: {PersonId}", person.Id);

            return Result.Success();
        }

        public async Task<IReadOnlyList<PersonResponse>> GetAllPersonsAsync()
        {
            _logger.LogInformation("GetAllPersons of PersonService");
            var allPersons = await _personsRepository.GetAllAsync();
            return allPersons.Select(person => person.ToPersonResponse()).ToList();
        }

        public async Task<Result<PersonResponse>> GetPersonAsync(Guid personId)
        {
            _logger.LogInformation("GetPerson of PersonService");

            // Get the person and validate existence
            var person = await _personsRepository.GetByIdAsync(personId);
            if(person is null)
            {
                return Result<PersonResponse>.Failure(PersonErrors.NotFound(personId));
            }

            return Result<PersonResponse>.Success(person.ToPersonResponse());
        }

        public async Task<Result<IReadOnlyList<PersonResponse>>> GetFilteredPersonsAsync(GetPersonRequest request)
        {
            _logger.LogInformation("GetFilteredPersons of PersonService");

            var validatorResult = await _getPersonRequestValidator.ValidateAsync(request);
            if(!validatorResult.IsValid)
            {
                return Result<IReadOnlyList<PersonResponse>>.Failure(validatorResult.ToErrorList());
            }

            // Get filtered persons
            var filteredPersons = await _personsRepository.GetFilteredPersonsAsync(
                request.SearchColumn, 
                request.SearchTerm, 
                request.SortColumn, 
                request.SortOrder);

            var resultValue = filteredPersons.Select(person => person.ToPersonResponse()).ToList();
            return Result<IReadOnlyList<PersonResponse>>.Success(resultValue);
        }

        public async Task<Result> DeletePersonAsync(Guid personId)
        {
            if(personId == Guid.Empty)
            {
                return Result.Failure(PersonErrors.EmptyId());
            }            

            // Strict Validation: Ensure the person exists before attempting deletion
            var person = await _personsRepository.GetByIdAsync(personId);
            if(person is null)
            {
                _logger.LogWarning("Attempted to delete a person that does not exist. Person Id: {PersonId}.", personId);
                return Result.Failure(PersonErrors.NotFound(personId));
            }

            // Validate deletion
            var isDeleted = await _personsRepository.DeleteAsync(person);
            if(!isDeleted)
            {
                var error = new Error("person.deletionfailed", $"Failed to delete Person with ID {personId}.", ErrorType.Failure);
                return Result.Failure(error);
            }

            LogDeletePerson(personId);

            return Result.Success();
        }

        [LoggerMessage(Level = LogLevel.Information, Message = "Deleting person with ID: {PersonId}")]
        private partial void LogDeletePerson(Guid personId);
    }
}
