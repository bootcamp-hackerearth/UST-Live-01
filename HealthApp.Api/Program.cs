using HealthApp.Api.Consumers;
using HealthApp.Api.Data;
using HealthApp.Api.Handler;
using HealthApp.Api.Mapping;
using HealthApp.Api.Models;
using HealthApp.Api.Options;
using HealthApp.Api.Repositories.Impl;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Dependencies;
using HealthApp.Api.Services.Impl;
using HealthApp.Api.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Serilog
// Reads Console, File, and Elasticsearch sinks from appsettings.json.
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HealthApp API",
        Version = "v1"
    });

    options.AddSecurityDefinition(
        "bearer",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter JWT token only. Do not type Bearer."
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(
                "bearer",
                document)] = []
        });
});

// Database
builder.Services.AddDbContext<HealthAppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"));
});

// Identity with ApplicationUser
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<HealthAppDbContext>()
    .AddDefaultTokenProviders();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowBlazor",
        policy =>
        {
            policy
                .WithOrigins(
                    "https://localhost:7028",
                    "http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// JWT Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        var jwtKey = jwt["Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException(
                "JWT signing key is not configured.");
        }

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwt["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwt["Audience"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.Zero
            };
    });

// Authorization
builder.Services.AddAuthorization();

// Global exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<
    IAuthorizationMiddlewareResultHandler,
    CustomAuthorizationMiddlewareResultHandler>();

// Repository registrations
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IDoctorLeaveRepository, DoctorLeaveRepository>();

// Grouped service dependencies
builder.Services.AddScoped<AppointmentServiceRepositories>();
builder.Services.AddScoped<DoctorLeaveServiceDependencies>();

// Service registrations
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDoctorLeaveService, DoctorLeaveService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Hosted services
builder.Services.AddHostedService<NotificationCleanupService>();
builder.Services.AddHostedService<HeartbeatService>();

// MassTransit / RabbitMQ
builder.Services.AddMassTransit(configuration =>
{
    configuration.AddConsumer<AppointmentBookedConsumer>();
    configuration.AddConsumer<AppointmentCancelledByDoctorLeaveConsumer>();

    configuration.UsingRabbitMq(
        (context, rabbitConfig) =>
        {
            var rabbitMqSection =
                builder.Configuration.GetSection("RabbitMq");

            var host = rabbitMqSection["Host"] ?? "localhost";
            var virtualHost = rabbitMqSection["VirtualHost"] ?? "/";
            var username = rabbitMqSection["Username"] ?? "guest";
            var password = rabbitMqSection["Password"] ?? "guest";

            rabbitConfig.Host(
                host,
                virtualHost,
                hostConfiguration =>
                {
                    hostConfiguration.Username(username);
                    hostConfiguration.Password(password);
                });

            rabbitConfig.ConfigureEndpoints(context);
        });
});

// Garnet / Redis distributed cache
builder.Services.Configure<GarnetOptions>(
    builder.Configuration.GetSection("Garnet"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    var garnetOptions = builder.Configuration
        .GetSection("Garnet")
        .Get<GarnetOptions>() ?? new GarnetOptions();

    options.Configuration = garnetOptions.ConnectionString;
    options.InstanceName = garnetOptions.InstanceName;
});

// AutoMapper
builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddProfile<MappingProfile>();
});

var app = builder.Build();

try
{
    Log.Information(
        "Starting HealthApp API in {Environment}",
        app.Environment.EnvironmentName);

    // Seed roles and admin user
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        var configuration = scope.ServiceProvider
            .GetRequiredService<IConfiguration>();

        await RoleSeeder.SeedRolesAndAdminAsync(
            roleManager,
            userManager,
            configuration);
    }

    // Seed login users for seeded doctors and patients
    await DemoUserSeeder.SeedDoctorAndPatientUsersAsync(app.Services);

    // Swagger / OpenAPI
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Record one log entry for each HTTP request.
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} " +
            "responded {StatusCode} in " +
            "{Elapsed:0.0000} ms";
    });

    app.UseHttpsRedirection();
    app.UseExceptionHandler();
    app.UseCors("AllowBlazor");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    Log.Information("HealthApp API started successfully");

    await app.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(
        exception,
        "HealthApp API terminated unexpectedly");
    Environment.ExitCode = 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}