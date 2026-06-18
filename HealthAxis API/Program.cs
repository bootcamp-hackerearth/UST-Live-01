using HealthAxis.API.Data;
using HealthAxis.API.Mappings;
using HealthAxis.API.Middleware;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

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

// Register AutoMapper.
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

// Register DbContext.
builder.Services.AddDbContext<HealthAxisDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HealthAxisDb"));
});

// Register Identity.
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

// Register JWT authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwt["Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)),

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
                "https://localhost:7273",
                "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register repositories.
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

// Register services.
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

var app = builder.Build();

// Global exception handling.
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => new
{
    Application = "HealthAxis API",
    Status = "Running",
    Message = "Welcome to HealthAxis API"
});

app.Run();