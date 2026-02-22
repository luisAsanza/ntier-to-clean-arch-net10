namespace CleanCRUDSolution.Application.Abstractions
{
    /// <summary>
    /// Abstraction for retrieving information about the currently authenticated user.
    /// Implement this in the infrastructure/web project to provide runtime values.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the current user's identifier, or Guid.Empty if not authenticated.
        /// </summary>
        Guid UserId { get; }

        /// <summary>
        /// Gets the current user's username, or an empty string if not available.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Indicates whether a user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
    }
}
