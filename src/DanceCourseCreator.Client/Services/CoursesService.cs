using System.Net.Http.Json;
using System.Net.Http.Headers;
using DanceCourseCreator.Client.Models;

namespace DanceCourseCreator.Client.Services;

public interface ICoursesService
{
    Task<List<Course>?> GetCoursesAsync(string? level = null, string? search = null);
    Task<Course?> GetCourseAsync(string id);
    Task<Course?> CreateCourseAsync(CreateCourseRequest request);
    Task<Course?> UpdateCourseAsync(string id, Course request);
    Task<bool> DeleteCourseAsync(string id);
    Task<object?> GetCourseCoverageAsync(string id);
}

public class CoursesService : ICoursesService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public CoursesService(HttpClient httpClient, IAuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<Course>?> GetCoursesAsync(string? level = null, string? search = null)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            var queryParams = new List<string>();
            
            if (!string.IsNullOrEmpty(level))
                queryParams.Add($"level={Uri.EscapeDataString(level)}");
                
            if (!string.IsNullOrEmpty(search))
                queryParams.Add($"search={Uri.EscapeDataString(search)}");
            
            var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
            var response = await _httpClient.GetAsync($"/api/courses{queryString}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Course>>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Course?> GetCourseAsync(string id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"/api/courses/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Course>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Course?> CreateCourseAsync(CreateCourseRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PostAsJsonAsync("/api/courses", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Course>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Course?> UpdateCourseAsync(string id, Course request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PutAsJsonAsync($"/api/courses/{id}", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Course>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteCourseAsync(string id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.DeleteAsync($"/api/courses/{id}");
            
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<object?> GetCourseCoverageAsync(string id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"/api/courses/{id}/coverage");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<object>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }
}