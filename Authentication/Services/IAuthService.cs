using Authentication.Entities;
using Authentication.Models;

namespace Authentication.Services
{
    public interface IAuthService
    {
        Task <User?> RegisterAsync(UserDto request);
        Task <TokenResponseDto?> LoginAsync(UserDto request);
        Task <TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}
