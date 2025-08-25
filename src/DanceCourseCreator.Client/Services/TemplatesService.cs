using System.Net.Http.Json;
using System.Net.Http.Headers;
using DanceCourseCreator.Client.Models;

namespace DanceCourseCreator.Client.Services;

public interface ITemplatesService
{
    Task<TemplateListResponse?> GetTemplatesAsync(string? scope = null, string? search = null, string? teamId = null, int page = 1, int pageSize = 20);
    Task<Template?> GetTemplateAsync(string id);
    Task<Template?> CreateTemplateAsync(CreateTemplateRequest request);
    Task<Template?> UpdateTemplateAsync(string id, UpdateTemplateRequest request);
    Task<bool> DeleteTemplateAsync(string id);
    Task<DuplicateTemplateResponse?> DuplicateTemplateAsync(string id, DuplicateTemplateRequest request);
    Task<List<Template>?> GetTeamTemplatesAsync(string teamId);
}

public class TemplatesService : ITemplatesService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public TemplatesService(HttpClient httpClient, IAuthService authService)
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

    public async Task<TemplateListResponse?> GetTemplatesAsync(string? scope = null, string? search = null, string? teamId = null, int page = 1, int pageSize = 20)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            
            var queryParams = new List<string>();
            
            if (!string.IsNullOrEmpty(scope))
                queryParams.Add($"scope={Uri.EscapeDataString(scope)}");
                
            if (!string.IsNullOrEmpty(search))
                queryParams.Add($"search={Uri.EscapeDataString(search)}");
                
            if (!string.IsNullOrEmpty(teamId))
                queryParams.Add($"teamId={Uri.EscapeDataString(teamId)}");
                
            queryParams.Add($"page={page}");
            queryParams.Add($"pageSize={pageSize}");
            
            var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
            
            var response = await _httpClient.GetFromJsonAsync<TemplateListResponse>($"api/templates{queryString}");
            return response;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error getting templates: {ex.Message}");
            return null;
        }
    }

    public async Task<Template?> GetTemplateAsync(string id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            return await _httpClient.GetFromJsonAsync<Template>($"api/templates/{id}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error getting template: {ex.Message}");
            return null;
        }
    }

    public async Task<Template?> CreateTemplateAsync(CreateTemplateRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PostAsJsonAsync("api/templates", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Template>();
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error creating template: {response.StatusCode} - {errorContent}");
            return null;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error creating template: {ex.Message}");
            return null;
        }
    }

    public async Task<Template?> UpdateTemplateAsync(string id, UpdateTemplateRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/templates/{id}", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Template>();
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error updating template: {response.StatusCode} - {errorContent}");
            return null;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error updating template: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteTemplateAsync(string id)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.DeleteAsync($"api/templates/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error deleting template: {ex.Message}");
            return false;
        }
    }

    public async Task<DuplicateTemplateResponse?> DuplicateTemplateAsync(string id, DuplicateTemplateRequest request)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.PostAsJsonAsync($"api/templates/{id}/duplicate", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<DuplicateTemplateResponse>();
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error duplicating template: {response.StatusCode} - {errorContent}");
            return null;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error duplicating template: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Template>?> GetTeamTemplatesAsync(string teamId)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            return await _httpClient.GetFromJsonAsync<List<Template>>($"api/templates/team/{teamId}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error getting team templates: {ex.Message}");
            return null;
        }
    }
}