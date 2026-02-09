using CleanCRUDSolution.Application.Common.Enums;
using CleanCRUDSolution.Application.Features.Persons.Enums;

namespace CleanCRUDSolution.Application.Features.Persons.DTOs
{
    public sealed record GetPersonRequest
    {
        public PersonColumn? SearchColumn { get; init; }
        public string? SearchTerm { get; init; }
        public PersonColumn? SortColumn { get; init; }
        public SortOrder? SortOrder { get; init; }
    }
}
