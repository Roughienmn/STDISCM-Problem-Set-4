using OurLasalle.Models;
namespace OurLasalle.ApiClients
{
    public class AuthServiceClient
    {
        private readonly HttpClient _httpClient;

        public AuthServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TokenResponseDto?> LoginAsync(UserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", userDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TokenResponseDto>();
            }
            return null;
        }

        public async Task<User?> RegisterAsync(UserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", userDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }
            return null;
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/refresh", refreshTokenRequestDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TokenResponseDto>();
            }
            return null;
        }
    }
}
