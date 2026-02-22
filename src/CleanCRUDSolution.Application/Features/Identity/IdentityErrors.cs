using CleanCRUDSolution.Application.Common.Results;

namespace CleanCRUDSolution.Application.Features.Identity;

/// <summary>
/// Contains error messages related to identity operations.
/// </summary>
public static class IdentityErrors
{
    public static Error Forbidden() => new("Access.Forbidden", "You do not have permission to perform this action.", ErrorType.Forbidden);
}