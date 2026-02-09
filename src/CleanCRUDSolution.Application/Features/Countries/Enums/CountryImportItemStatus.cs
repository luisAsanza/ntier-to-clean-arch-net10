namespace CleanCRUDSolution.Application.Features.Countries.Enums
{
    public enum CountryImportItemStatus
    {
        Valid,
        Invalid,
        DuplicateInFile,
        DuplicateInDb,
        BlockedByPolicy,
        FailedToPersist,
        Persisted
    }
}
