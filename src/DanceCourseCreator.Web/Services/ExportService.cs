using System.Net.Http.Headers;
using System.Net.Http.Json;
using DanceCourseCreator.Web.Models;
using Microsoft.JSInterop;

namespace DanceCourseCreator.Web.Services;

public interface IExportService
{
    Task ExportCourseAsync(string courseId, ExportFormat format, bool includeSchedule = true, bool includePatternDetails = true);
    Task ExportLessonAsync(string lessonId, ExportFormat format, bool includePatternDetails = true);
}

public class ExportService : IExportService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;
    private readonly IJSRuntime _jsRuntime;

    public ExportService(HttpClient httpClient, IAuthService authService, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _authService = authService;
        _jsRuntime = jsRuntime;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task ExportCourseAsync(string courseId, ExportFormat format, bool includeSchedule = true, bool includePatternDetails = true)
    {
        await EnsureAuthenticatedAsync();

        var request = new ExportRequest
        {
            Format = format,
            IncludeSchedule = includeSchedule,
            IncludePatternDetails = includePatternDetails
        };

        var response = await _httpClient.PostAsJsonAsync($"api/export/course/{courseId}", request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsByteArrayAsync();
        var fileName = $"course-{courseId}.{GetFileExtension(format)}";
        var mimeType = GetMimeType(format);

        await DownloadFileAsync(fileName, content, mimeType);
    }

    public async Task ExportLessonAsync(string lessonId, ExportFormat format, bool includePatternDetails = true)
    {
        await EnsureAuthenticatedAsync();

        var request = new ExportRequest
        {
            Format = format,
            IncludePatternDetails = includePatternDetails
        };

        var response = await _httpClient.PostAsJsonAsync($"api/export/lesson/{lessonId}", request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsByteArrayAsync();
        var fileName = $"lesson-{lessonId}.{GetFileExtension(format)}";
        var mimeType = GetMimeType(format);

        await DownloadFileAsync(fileName, content, mimeType);
    }

    private static string GetFileExtension(ExportFormat format)
    {
        return format switch
        {
            ExportFormat.PDF => "pdf",
            ExportFormat.Markdown => "md",
            _ => "txt"
        };
    }

    private static string GetMimeType(ExportFormat format)
    {
        return format switch
        {
            ExportFormat.PDF => "application/pdf",
            ExportFormat.Markdown => "text/markdown",
            _ => "text/plain"
        };
    }

    private async Task DownloadFileAsync(string fileName, byte[] content, string mimeType)
    {
        var base64 = Convert.ToBase64String(content);
        await _jsRuntime.InvokeVoidAsync("downloadFile", fileName, base64, mimeType);
    }
}
