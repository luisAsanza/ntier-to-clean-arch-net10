using System.IO.Pipelines;
using CleanCRUDSolution.Application.Common.Results;

namespace CleanCRUDSolution.Application.Features.Identity
{
    public interface IIdentityService
    {
        Task<Result<Guid>> CreateUserAsync(string email, string password);
        
        Task<Result<bool>> CheckPasswordAsync(string email, string password);
        
        Task<Result<string>> GetUserNameAsync(Guid userId);
    }
}