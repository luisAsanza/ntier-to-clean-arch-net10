using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Identity;
using Microsoft.AspNetCore.Identity;

namespace CleanCRUDSolution.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<Guid>> CreateUserAsync(string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error("Identity.CreateUserFailed", e.Description, ErrorType.Failure));
                return Result<Guid>.Failure(errors);
            }

            return Result<Guid>.Success(user.Id);
        }

        public async Task<Result<bool>> CheckPasswordAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<bool>.Failure(new Error("Identity.UserNotFound", "User not found.", ErrorType.NotFound));
            }

            var isValid = await _userManager.CheckPasswordAsync(user, password);
            return Result<bool>.Success(isValid);
        }

        public async Task<Result<string>> GetUserNameAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Result<string>.Failure(new Error("Identity.UserNotFound", "User not found.", ErrorType.NotFound));
            }

            return Result<string>.Success(user.UserName!);
        }
    }
}
