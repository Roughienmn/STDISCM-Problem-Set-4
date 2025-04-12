using OurLasalle.Models;

namespace OurLasalle.ApiClients
{
    public class GradeServiceClient
    {
        private readonly HttpClient _httpClient;
        public GradeServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<GradeDto>> GetGradesByCourseIdAsync(Guid courseId)
        {
            // Fetch grades from the Grades API
            var response = await _httpClient.GetAsync($"api/Grade/course/{courseId}");
            if (!response.IsSuccessStatusCode)
            {
                return new List<GradeDto>();
            }

            var grades = await response.Content.ReadFromJsonAsync<List<GradeDto>>() ?? new List<GradeDto>();

            // Fetch the course details from the Courses API
            var courseServiceClient = new CourseServiceClient(_httpClient);
            var course = await courseServiceClient.GetCourseByIdAsync(courseId);

            // Add the CourseName to each grade
            if (course != null)
            {
                foreach (var grade in grades)
                {
                    grade.CourseName = course.Name;
                }
            }

            return grades;
        }

        public async Task<List<GradeDto>> GetGradesByStudentIdAsync(Guid studentId)
        {
            var response = await _httpClient.GetAsync($"api/Grade/student/{studentId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<GradeDto>>() ?? new List<GradeDto>();
            }
            return new List<GradeDto>();
        }
        public async Task<bool> AddGradeAsync(AddGradeRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Grade/addgrade", request);
            return response.IsSuccessStatusCode;
        }
    }
}
