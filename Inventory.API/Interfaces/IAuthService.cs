using Inventory.API.Models;

namespace Inventory.API.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(LoginModel loginModel);
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<string> GenerateJwtToken(string username);
    }
}
