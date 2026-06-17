using HealthAxis_Health.API.Helpers;
using HealthAxisHealth.API.Configurations;
using HealthAxisHealth.API.Data;
using HealthAxisHealth.API.Handlers;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Mappings;
using HealthAxisHealth.API.Repositories.Implementations;
using HealthAxisHealth.API.Repositories.Interfaces;
using HealthAxisHealth.API.Services.Implementations;
using HealthAxisHealth.API.Services.Interfaces;
using HealthAxisHealth.API.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Controllers & Swagger

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "HealthAxis Health API",
            Version = "v1",
            Description = "Healthcare Appointment Management API"
        });

    options.AddSecurityDefinition(
        "bearer",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description =
                "Enter JWT token. Example: Bearer eyJhbGciOiJIUzI1NiIs..."
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(
                "bearer",
                document)] = []
        });
});

#endregion

#region Database

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection")));

#endregion
#region CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7273", // Blazor Admin
                "http://localhost:4200")  // Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


#endregion

#region JWT Configuration

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(
        nameof(JwtSettings)));

JwtSettings jwtSettings =
    builder.Configuration
        .GetSection(nameof(JwtSettings))
        .Get<JwtSettings>()
    ?? throw new InvalidOperationException(
        "JWT settings are missing.");

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    jwtSettings.Issuer,

                ValidAudience =
                    jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.SecretKey)),

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

#endregion

#region Helpers

builder.Services.AddScoped<
    IJwtTokenGenerator,
    JwtTokenGenerator>();

#endregion

#region Repositories

builder.Services.AddScoped<
    IUserRepository,
    UserRepository>();

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

#endregion

#region Unit Of Work

builder.Services.AddScoped<
    IUnitOfWork,
    UnitOfWork>();

#endregion

#region Services

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

builder.Services.AddExceptionHandler<
    GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

#endregion

var app = builder.Build();

#region Middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle =
            "HealthAxis Health API";
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

#endregion

await app.RunAsync();