using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using uts.Models;

namespace uts
{
    internal class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/api/");
        }
        public async Task<List<Categories>> GetCategoriesAsync()
        {


            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.AuthToken);
            var categories = await _httpClient.GetFromJsonAsync<List<Categories>>("categories");

            if (categories != null)
            {
                Console.WriteLine("Categories retrieved successfully:");
                foreach (var category in categories)
                {
                    Console.WriteLine($"- {category}");
                }
            }
            else
            {
                Console.WriteLine("No categories found.");
            }

            return categories;
        }
        public async Task<List<Courses>> GetCoursesAsync()
        {
            // Ambil data courses
            var courses = await _httpClient.GetFromJsonAsync<List<Courses>>("Courses");

            // Ambil data enrollments
            var enrollments = await _httpClient.GetFromJsonAsync<List<Enrollments>>("https://actbackendseervices.azurewebsites.net/api/enrollments");

            // Ambil data instructors
            var instructors = await _httpClient.GetFromJsonAsync<List<Instructors>>("https://actbackendseervices.azurewebsites.net/api/instructors");

            if (courses != null)
            {
                foreach (var course in courses)
                {
                    // Hitung jumlah enrollments untuk setiap course
                    course.EnrollmentCount = enrollments?.Count(e => e.CourseId == course.courseId) ?? 0;

                    // Dapatkan semua instructorId yang terdaftar untuk course ini
                    var instructorIds = enrollments?
                        .Where(e => e.CourseId == course.courseId)
                        .Select(e => e.InstructorId)
                        .Distinct()
                        .ToList();

                    // Cari semua nama instruktur berdasarkan instructorId
                    if (instructorIds != null)
                    {
                        var instructorNames = instructors?
                            .Where(i => instructorIds.Contains(i.instructorId))
                            .Select(i => i.fullName)
                            .ToList();

                        // Gabungkan nama-nama instruktur dengan koma
                        course.InstructorName = instructorNames != null
                            ? string.Join(", ", instructorNames)
                            : string.Empty;
                    }
                    else
                    {
                        course.InstructorName = string.Empty;
                    }
                }
            }

            return courses;
        }
        public async Task<bool> PostCategoryAsync(Categories category)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("categories", category);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AssignRoleAsync(object roleAssignment)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("registeruserrole", roleAssignment);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PostUsersAsync(Users user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("users", user);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> PostCourseAsync(Courses courses)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Courses", courses);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> PutCategoryAsync(Categories category)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"categories/{category.categoryId}", category);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> PutCourseAsync(Courses course)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Courses/{course.courseId}", course);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        public async Task<bool> PutCourseWithCategoryIdAsync(Courses course, int categoryId)
        {
            try
            {
                // Objek payload hanya dengan categoryId
                var payload = new
                {
                    course.courseId,
                    course.name,
                    course.imageName,
                    course.duration,
                    course.description,
                    categoryId
                };

                // Serialisasi payload ke JSON
                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Kirim permintaan PUT ke server
                var response = await _httpClient.PutAsync($"Courses/{course.courseId}", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PutCourseWithCategoryIdAsync: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"categories/{categoryId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Courses/{courseId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        public async Task<Categories> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"categories/{categoryId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Categories>();
                }
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }
        public async Task<Courses> GetCourseByIdAsync(int courseId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Courses/{courseId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Courses>();
                }
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }
        public async Task<Courses?> GetCourseByNameAsync(string courseName)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Courses/search/{courseName}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Courses>();
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<bool> PostEnrollmentAsync(object enrollmentPayload)
        {
            var response = await _httpClient.PostAsJsonAsync("https://actbackendseervices.azurewebsites.net/api/enrollments", enrollmentPayload);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Instructors>> GetInstructorsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Instructors>>("https://actbackendseervices.azurewebsites.net/api/instructors");
        }

        public async Task<List<Users>> GetUsersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Users>>("https://actbackendseervices.azurewebsites.net/api/users");
            return response ?? new List<Users>();
        }

        public async Task<List<UserInstructorViewModel>> GetUserInstructorDataAsync()
        {
            var users = await GetUsersAsync();
            var instructors = await GetInstructorsAsync();

            var result = (from user in users
                          join instructor in instructors
                          on user.UserName equals instructor.userName
                          select new UserInstructorViewModel
                          {
                              Id = user.Id,
                              Email = user.Email,
                              UserName = user.UserName,
                              FullName = user.FullName,
                              InstructorId = instructor.instructorId
                          }).ToList();

            return result;
        }




    }


}
