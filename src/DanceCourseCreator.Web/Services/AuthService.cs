using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using DanceCourseCreator.Web.Models;

namespace DanceCourseCreator.Web.Services;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<User?> GetProfileAsync();
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
    Task SetTokenAsync(string token);
    Task RemoveTokenAsync();
}

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private const string TOKEN_KEY = "authToken";
    private string? _cachedToken;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);
            
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse != null)
                {
                    await SetTokenAsync(authResponse.Token);
                    SetAuthorizationHeader(authResponse.Token);
                }
                return authResponse;
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);
            
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse != null)
                {
                    await SetTokenAsync(authResponse.Token);
                    SetAuthorizationHeader(authResponse.Token);
                }
                return authResponse;
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<User?> GetProfileAsync()
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;

            SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync("/api/auth/profile");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        await RemoveTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            // During prerendering, return cached token if available
            if (_cachedToken != null)
                return _cachedToken;
                
            var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TOKEN_KEY);
            if (!string.IsNullOrEmpty(token))
                _cachedToken = token;
            return token;
        }
        catch (InvalidOperationException)
        {
            // JavaScript interop not available during prerendering
            return _cachedToken;
        }
    }

    public async Task SetTokenAsync(string token)
    {
        _cachedToken = token;
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, token);
        }
        catch (InvalidOperationException)
        {
            // JavaScript interop not available during prerendering, token is cached
        }
    }

    public async Task RemoveTokenAsync()
    {
        _cachedToken = null;
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
        }
        catch (InvalidOperationException)
        {
            // JavaScript interop not available during prerendering, cache is cleared
        }
    }

    private void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}