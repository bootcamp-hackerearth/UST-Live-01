using HealthAxis.API.Data;
using HealthAxis.API.Mappings;
using HealthAxis.API.Middleware;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using HealthAxis.API.BackgroundServices;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/healthaxis-api-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddHostedService<HealthAxisHeartbeatService>();
builder.Services.AddHostedService<NotificationCleanupService>();

// Add controllers.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            JsonNamingPolicy.CamelCase;
    });

// Global exception handler.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// AutoMapper.
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

// DbContext.
builder.Services.AddDbContext<HealthAxisDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HealthAxisDb"));
});

// Identity.
// Roles are not seeded because patient role is stored as ClaimTypes.Role.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<HealthAxisDbContext>()
.AddDefaultTokenProviders();

// Important: prevent Identity cookie redirects for API requests.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

// JWT Authentication.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    IConfigurationSection jwt =
        builder.Configuration.GetSection("Jwt");

    string jwtKey =
        jwt["Key"]
        ?? throw new InvalidOperationException("JWT Key is missing.");

    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwt["Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            RoleClaimType = ClaimTypes.Role,

            ClockSkew = TimeSpan.Zero
        };
});

builder.Services.AddAuthorization();

// CORS for Angular and Blazor frontends.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7051", // Blazor WebAssembly Admin HTTPS
                "http://localhost:5293",  // Blazor WebAssembly Admin HTTP
                "http://localhost:4200")  // Angular frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "HealthAxis API",
            Version = "v1",
            Description = "API for HealthAxis Healthcare System"
        });

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Paste only the JWT token. Do not type the word Bearer manually."
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});

// Repositories.
builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));

builder.Services.AddScoped<
    IPatientRepository,
    PatientRepository>();

builder.Services.AddScoped<
    IDoctorRepository,
    DoctorRepository>();

builder.Services.AddScoped<
    IAppointmentRepository,
    AppointmentRepository>();

builder.Services.AddScoped<
    IHealthRecordRepository,
    HealthRecordRepository>();

// Services.
builder.Services.AddScoped<
    IAuthService,
    AuthService>();

builder.Services.AddScoped<
    IPatientService,
    PatientService>();

builder.Services.AddScoped<
    IDoctorService,
    DoctorService>();

builder.Services.AddScoped<
    IAppointmentService,
    AppointmentService>();

builder.Services.AddScoped<
    IHealthRecordService,
    HealthRecordService>();

builder.Services.AddScoped<
    IAdminService,
    AdminService>();

var app = builder.Build();

// Global exception handling.
app.UseExceptionHandler();

// Swagger middleware.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "HealthAxis API v1");

        options.RoutePrefix = string.Empty;
    });
}

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
