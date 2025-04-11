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
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/login", userDto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TokenResponseDto>();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error in LoginAsync: {ex.Message}");
            }
            return null;
        }

        public async Task<User?> RegisterAsync(UserDto userDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/register", userDto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<User>();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error in RegisterAsync: {ex.Message}");
            }
            return null;
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/refresh", refreshTokenRequestDto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TokenResponseDto>();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error in RefreshTokensAsync: {ex.Message}");
            }
            return null;
        }
    }
}
