using DanceCourseCreator.Web.Components;
using DanceCourseCreator.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire ServiceDefaults
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add MudBlazor services
builder.Services.AddMudServices();

// Configure API HttpClient
var apiBaseUrl = builder.Configuration["services:api:https:0"] 
    ?? builder.Configuration["services:api:http:0"]
    ?? "http://localhost:5139";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

// Add application services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatternsService, PatternsService>();
builder.Services.AddScoped<ILessonsService, LessonsService>();
builder.Services.AddScoped<ICoursesService, CoursesService>();
builder.Services.AddScoped<ITemplatesService, TemplatesService>();
builder.Services.AddScoped<ITimeValidationService, TimeValidationService>();
builder.Services.AddScoped<IBreakpointService, BreakpointService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Add authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
