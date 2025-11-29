using System.Net.Http.Json;
using System.Net.Http.Headers;
using DanceCourseCreator.Client.Models;

namespace DanceCourseCreator.Client.Services;

public interface ILessonsService
{
    Task<List<Lesson>?> GetLessonsAsync(string? courseId = null);
    Task<Lesson?> GetLessonAsync(string id);
    Task<Lesson?> CreateLessonAsync(CreateLessonRequest request);
    Task<Lesson?> UpdateLessonAsync(string id, CreateLessonRequest request);
    Task<bool> DeleteLessonAsync(string id);
}

public class LessonsService : ILessonsService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public LessonsService(HttpClient httpClient, IAuthService authService)
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

    public async Task<List<Lesson>?> GetLessonsAsync(string? courseId = null)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            var queryString = !string.IsNullOrEmpty(courseId) ? $"?courseId={Uri.EscapeDataString(courseId)}" : "";
            var response = await _httpClient.GetAsync($"/api/lessons{queryString}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Lesson>>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Lesson?> GetLessonAsync(string id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"/api/lessons/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Lesson>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Lesson?> CreateLessonAsync(CreateLessonRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PostAsJsonAsync("/api/lessons", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Lesson>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Lesson?> UpdateLessonAsync(string id, CreateLessonRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PutAsJsonAsync($"/api/lessons/{id}", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Lesson>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteLessonAsync(string id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.DeleteAsync($"/api/lessons/{id}");
            
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}