using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using DanceCourseCreator.API.Data;
using DanceCourseCreator.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Entity Framework
builder.Services.AddDbContext<DanceCourseDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=database.sqlite"));

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JWT");
var secretKey = jwtSettings["SecretKey"] ?? "your-very-secure-secret-key-that-is-at-least-256-bits-long";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"] ?? "DanceCourseCreator",
            ValidAudience = jwtSettings["Audience"] ?? "DanceCourseCreatorAPI",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Add services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ProgressionService>();
builder.Services.AddScoped<ExportService>();

// Add controllers
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        policy =>
        {
            policy.WithOrigins("https://localhost:5001", "http://localhost:5000", "http://localhost:5034")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dance Course Creator API", Version = "v1" });
    
    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", null, null),
            new List<string>()
        }
    });
});

var app = builder.Build();

// Default route for controllers
app.MapDefaultEndpoints();

// Ensure database is created and seed demo user
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DanceCourseDbContext>();
    context.Database.EnsureCreated();
    
    // Ensure demo user exists for testing - with read-only access
    var demoEmail = "demo@dancecourse.com";
    var demoUser = await context.Users.FirstOrDefaultAsync(u => u.Email == demoEmail);
    if (demoUser == null)
    {
        demoUser = new DanceCourseCreator.API.Models.User
        {
            Id = "demo-user-id",
            Name = "Demo Användare",
            Email = demoEmail,
            Role = DanceCourseCreator.API.Models.UserRole.Reader,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("demo123"),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Users.Add(demoUser);
        await context.SaveChangesAsync();
    }
    else if (demoUser.Role != DanceCourseCreator.API.Models.UserRole.Reader)
    {
        // Update existing demo user to Reader role if it has a different role
        demoUser.Role = DanceCourseCreator.API.Models.UserRole.Reader;
        demoUser.Name = "Demo Användare";
        demoUser.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
    }
    
    // Seed West Coast Swing test data
    await WestCoastSwingSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorClient");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapGet("/api/health", () => new { status = "OK", timestamp = DateTime.UtcNow })
    .WithName("GetHealth");

// Log application started with timestamp
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Application started at {StartTime}", DateTime.UtcNow);
});

app.Run();

// Make Program accessible for integration tests
public partial class Program { }
