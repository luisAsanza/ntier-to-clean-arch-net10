namespace CleanCRUDSolution.Application.Features.Countries.Enums
{
    public enum ImportOutcome
    {
        NotAttempted,
        Persisted,
        BlockedByPolicy,
        FailedToPersist
    }
}
