﻿using Courses.Entities;
using Courses.Models;

namespace Courses.Services
{
    public interface ICourseService
    {
        Task<CourseDto?> CreateCourseAsync(CreateCourseRequest request);
        Task<List<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(Guid id);
        Task<List<CourseDto>> GetCoursesByTeacherIdAsync(Guid teacherId);

        Task<bool> EnrollStudentAsync(EnrollRequestDto request);
    }
}