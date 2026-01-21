using CleanCRUDSolution.Application.Common.Results;

namespace CleanCRUDSolution.Application.Features.Persons
{
    public static class PersonErrors
    {
        public static Error NotFound(Guid id) => new("Person.NotFound", $"Person with ID '{id}' was not found.", ErrorType.NotFound);

        public static Error EmptyId() => new("personid.empty", $"Person Id is empty.", ErrorType.Validation);
    }
}
