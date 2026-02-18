using Microsoft.AspNetCore.Identity;

namespace CleanCRUDSolution.Infrastructure.Identity
{
    /// <summary>
    /// Represents an application user used exclusively for authentication and authorization concerns.
    /// Inherits from Microsoft.AspNetCore.Identity.IdentityUser.
    /// </summary>
    /// <remarks>
    /// This type should only contain identity/authentication related properties. Do NOT add business/domain data here.
    /// Suggested auth-only properties (examples of properties to keep or add):
    /// - DateTime? LastLoginUtc: UTC timestamp of the last successful sign-in.
    /// - DateTime? PasswordLastChangedUtc: UTC timestamp when the password was last changed.
    /// - string? AuthenticatorKey: Key used by authenticator apps (TOTP/HOTP).
    /// - IReadOnlyCollection<string>? RecoveryCodes: One-time recovery codes for two-factor recovery.
    /// - bool IsLockedOut: Indicates whether the account is currently locked out.
    /// - DateTimeOffset? LockoutEndUtc: UTC time when the lockout ends.
    /// - string SecurityStamp: Stamp used to invalidate authentication tokens (inherited from IdentityUser).
    /// - string ConcurrencyStamp: Concurrency token for identity updates (inherited).
    ///
    /// Place profile, business or domain-specific properties (customer data, billing, preferences, etc.)
    /// in separate domain/profile models or related tables.
    /// </remarks>
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
}