namespace CleanCRUDSolution.Application.Common.Results
{
    public sealed record Error(
        string Code,
        string Message,
        ErrorType Type = ErrorType.Failure,
        string? Field = null
        );
}
