using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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
            _httpClient.BaseAddress = new Uri("https://actualbackendapp.azurewebsites.net/api/");
        }
        public async Task<List<Categories>> GetCategoriesAsync()
        {
            var categories = await _httpClient.GetFromJsonAsync<List<Categories>>("v1/Categories");

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
                var response = await _httpClient.PostAsJsonAsync("v1/Categories", category);
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
                var response = await _httpClient.PutAsJsonAsync($"v1/Categories/{category.categoryId}", category);
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
        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"v1/Categories/{categoryId}");
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
                var response = await _httpClient.DeleteAsync($"v1/Courses/{courseId}");
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
                var response = await _httpClient.GetAsync($"v1/Categories/{categoryId}");
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

    }


}
