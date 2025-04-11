using OurLasalle.Models;

namespace OurLasalle.ApiClients
{
    public class CourseServiceClient
    {
        private readonly HttpClient _httpClient;

        public CourseServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CourseDto>?> GetAllCoursesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Course/all");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<CourseDto>>();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error in GetAllCoursesAsync: {ex.Message}");
            }
            return null;
        }

        public async Task<CourseDto?> CreateCourseAsync(CreateCourseRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Course/create", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CourseDto>();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error in CreateCourseAsync: {ex.Message}");
            }
            return null;
        }

        public async Task EnrollStudentAsync(EnrollRequestDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Course/enroll", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error in EnrollStudentAsync: {ex.Message}");
            }
        }
    }
}
