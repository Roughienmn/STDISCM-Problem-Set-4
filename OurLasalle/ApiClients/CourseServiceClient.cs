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
            var response = await _httpClient.GetAsync("api/Course/all");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CourseDto>>();
            }
            return null;
        }

        public async Task<CourseDto?> CreateCourseAsync(CreateCourseRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Course/create", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CourseDto>();
            }
            return null;
        }

        public async Task EnrollStudentAsync(EnrollRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Course/enroll", request);
            response.EnsureSuccessStatusCode();
        }
    }
}
