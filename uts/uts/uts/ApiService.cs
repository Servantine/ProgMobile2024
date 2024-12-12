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
            var courses = await _httpClient.GetFromJsonAsync<List<Courses>>("Courses");

            if (courses != null)
            {
                Console.WriteLine("Courses retrieved successfully:");
                foreach (var coursess in courses)
                {
                    Console.WriteLine($"- {coursess}");
                }
            }
            else
            {
                Console.WriteLine("No categories found.");
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

    }


}
