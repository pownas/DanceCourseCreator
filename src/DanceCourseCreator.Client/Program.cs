using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using DanceCourseCreator.Client;
using DanceCourseCreator.Client.Services;
using DanceCourseCreator.Client.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add Aspire ServiceDefaults for observability
builder.AddServiceDefaults();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure API HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5139") });

// Add MudBlazor services
builder.Services.AddMudServices();

// Add application services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatternsService, PatternsService>();
builder.Services.AddScoped<ILessonsService, LessonsService>();
builder.Services.AddScoped<ICoursesService, CoursesService>();
builder.Services.AddScoped<ITemplatesService, TemplatesService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IObservabilityTestService, ObservabilityTestService>();

// Add authorization
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
