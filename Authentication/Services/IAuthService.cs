using Authentication.Entities;
using Authentication.Models;

namespace Authentication.Services
{
    public interface IAuthService
    {
        Task <User?> RegisterAsync(UserDto request);
        Task <string?> LoginAsync(UserDto request);
    }
}
