using System.Threading.Tasks;

namespace CleanCRUDSolution.Application.Features.Identity
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, string UserId)> CreateUserAsync(string email, string password);
        
        Task<bool> CheckPasswordAsync(string email, string password);
        
        Task<string> GetUserNameAsync(string userId);
    }
}