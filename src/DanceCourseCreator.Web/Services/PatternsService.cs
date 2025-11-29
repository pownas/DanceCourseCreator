using System.Net.Http.Json;
using System.Net.Http.Headers;
using DanceCourseCreator.Client.Models;

namespace DanceCourseCreator.Client.Services;

public interface IPatternsService
{
    Task<List<PatternOrExercise>?> GetPatternsAsync(string? type = null, string? level = null, string? danceStyle = null, string? search = null, List<string>? tags = null);
    Task<PatternOrExercise?> GetPatternAsync(string id);
    Task<PatternOrExercise?> CreatePatternAsync(CreatePatternRequest request);
    Task<PatternOrExercise?> UpdatePatternAsync(string id, CreatePatternRequest request);
    Task<bool> DeletePatternAsync(string id);
}

public class PatternsService : IPatternsService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public PatternsService(HttpClient httpClient, IAuthService authService)
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

    public async Task<List<PatternOrExercise>?> GetPatternsAsync(string? type = null, string? level = null, string? danceStyle = null, string? search = null, List<string>? tags = null)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            var queryParams = new List<string>();
            
            if (!string.IsNullOrEmpty(type))
                queryParams.Add($"type={Uri.EscapeDataString(type)}");
                
            if (!string.IsNullOrEmpty(level))
                queryParams.Add($"level={Uri.EscapeDataString(level)}");
            
            if (!string.IsNullOrEmpty(danceStyle))
                queryParams.Add($"danceStyle={Uri.EscapeDataString(danceStyle)}");
                
            if (!string.IsNullOrEmpty(search))
                queryParams.Add($"search={Uri.EscapeDataString(search)}");
                
            if (tags != null && tags.Any())
            {
                foreach (var tag in tags)
                {
                    queryParams.Add($"tags={Uri.EscapeDataString(tag)}");
                }
            }
            
            var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
            var response = await _httpClient.GetAsync($"/api/patterns{queryString}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<PatternOrExercise>>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<PatternOrExercise?> GetPatternAsync(string id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync($"/api/patterns/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PatternOrExercise>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<PatternOrExercise?> CreatePatternAsync(CreatePatternRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PostAsJsonAsync("/api/patterns", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PatternOrExercise>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<PatternOrExercise?> UpdatePatternAsync(string id, CreatePatternRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PutAsJsonAsync($"/api/patterns/{id}", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PatternOrExercise>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeletePatternAsync(string id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.DeleteAsync($"/api/patterns/{id}");
            
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}