namespace CleanCRUDSolution.Application.Common.Results
{
    /// <summary>
    /// Represents a single error with a code, message and optional field and type.
    /// Used across application layers to represent validation and operation failures.
    /// </summary>
    public sealed record Error(
        string Code,
        string Message,
        ErrorType Type = ErrorType.Failure,
        string? Field = null
        );
}
