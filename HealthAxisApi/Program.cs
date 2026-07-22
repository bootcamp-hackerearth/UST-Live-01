using System.Text;

using HealthAxisCore_Api.BackgroundServices;
using HealthAxisCore_Api.Consumers;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Mappings;
using HealthAxisCore_Api.Middleware;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Options;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Repositories.Implementation;
using HealthAxisCore_Api.Repositories.Interface;
using HealthAxisCore_Api.Services.Implementation;
using HealthAxisCore_Api.Services.Implementations;
using HealthAxisCore_Api.Services.Interfaces;

using MassTransit;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Serilog;
using Serilog.Events;

const string CorsPolicyName = "CorsPolicy";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting HealthAxis API.");

    var builder =
        WebApplication.CreateBuilder(args);

    ConfigureSerilog(builder);

    builder.Services.AddControllers();

    ConfigureDatabase(builder);
    ConfigureIdentity(builder);
    ConfigureAuthentication(builder);
    ConfigureDistributedCache(builder);
    ConfigureAutoMapper(builder);

    RegisterRepositories(builder.Services);
    RegisterApplicationServices(builder.Services);

    ConfigureMassTransit(builder);
    ConfigureHostOptions(builder.Services);
    RegisterBackgroundServices(builder.Services);

    ConfigureSwagger(builder.Services);
    ConfigureCors(builder.Services);

    var app = builder.Build();

    await SeedIdentityDataAsync(app.Services);

    ConfigureHttpPipeline(app);

    await app.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(
        exception,
        "HealthAxis API terminated unexpectedly.");
}
finally
{
    Log.Information("HealthAxis API stopped.");

    await Log.CloseAndFlushAsync();
}

static void ConfigureSerilog(
    WebApplicationBuilder builder)
{
    builder.Host.UseSerilog(
        (context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(
                    context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });
}

static void ConfigureDatabase(
    WebApplicationBuilder builder)
{
    var connectionString =
        builder.Configuration.GetConnectionString(
            "DefaultConnection");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException(
            "The DefaultConnection connection string is missing.");
    }

    builder.Services.AddDbContext<HealthAppDbContext>(
        options =>
        {
            options.UseSqlServer(connectionString);
        });
}

static void ConfigureIdentity(
    WebApplicationBuilder builder)
{
    builder.Services
        .AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<HealthAppDbContext>()
        .AddDefaultTokenProviders();
}

static void ConfigureAuthentication(
    WebApplicationBuilder builder)
{
    var jwtSettings =
        builder.Configuration.GetSection("Jwt");

    var jwtKey = jwtSettings["Key"];

    if (string.IsNullOrWhiteSpace(jwtKey))
    {
        throw new InvalidOperationException(
            "The JWT signing key is missing.");
    }

    var signingKey =
        Encoding.UTF8.GetBytes(jwtKey);

    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
        })
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
                        jwtSettings["Issuer"],

                    ValidAudience =
                        jwtSettings["Audience"],

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            signingKey),

                    ClockSkew = TimeSpan.Zero
                };
        });

    builder.Services.AddAuthorization();
}

static void ConfigureDistributedCache(
    WebApplicationBuilder builder)
{
    var garnetSection =
        builder.Configuration.GetSection(
            GarnetOptions.SectionName);

    var garnetConnectionString =
        garnetSection["ConnectionString"];

    if (string.IsNullOrWhiteSpace(
        garnetConnectionString))
    {
        throw new InvalidOperationException(
            "The Garnet connection string is missing.");
    }

    builder.Services.Configure<GarnetOptions>(
        garnetSection);

    builder.Services.AddStackExchangeRedisCache(
        options =>
        {
            options.Configuration =
                garnetConnectionString;

            options.InstanceName =
                garnetSection["InstanceName"];
        });
}

static void ConfigureAutoMapper(
    WebApplicationBuilder builder)
{
    builder.Services.AddAutoMapper(
        configuration =>
        {
            configuration.AddProfile<
                MappingProfile>();
        });
}

static void RegisterRepositories(
    IServiceCollection services)
{
    services.AddScoped(
        typeof(IGenericRepository<>),
        typeof(GenericRepository<>));

    services.AddScoped<
        IPatientRepository,
        PatientRepository>();

    services.AddScoped<
        IDoctorRepository,
        DoctorRepository>();

    services.AddScoped<
        IAppointmentRepository,
        AppointmentRepository>();

    services.AddScoped<
        IHealthRecordRepository,
        HealthRecordRepository>();

    services.AddScoped<
        IDoctorLeaveRepository,
        DoctorLeaveRepository>();
}

static void RegisterApplicationServices(
    IServiceCollection services)
{
    services.AddScoped<
        IPatientService,
        PatientService>();

    services.AddScoped<
        IDoctorService,
        DoctorService>();

    services.AddScoped<
        IAppointmentService,
        AppointmentService>();

    services.AddScoped<
        IHealthRecordService,
        HealthRecordService>();

    services.AddScoped<
        IDoctorLeaveService,
        DoctorLeaveService>();

    services.AddScoped<
        IAuthService,
        AuthService>();

    services.AddScoped<
        ICacheService,
        CacheService>();
}

static void ConfigureMassTransit(
    WebApplicationBuilder builder)
{
    var rabbitMqSettings =
        builder.Configuration.GetSection(
            "RabbitMQ");

    var host =
        rabbitMqSettings["Host"];

    var username =
        rabbitMqSettings["Username"];

    var password =
        rabbitMqSettings["Password"];

    if (string.IsNullOrWhiteSpace(host))
    {
        throw new InvalidOperationException(
            "The RabbitMQ host is missing.");
    }

    if (string.IsNullOrWhiteSpace(username))
    {
        throw new InvalidOperationException(
            "The RabbitMQ username is missing.");
    }

    if (string.IsNullOrWhiteSpace(password))
    {
        throw new InvalidOperationException(
            "The RabbitMQ password is missing.");
    }

    builder.Services.AddMassTransit(
        configuration =>
        {
            configuration.AddConsumer<
                AppointmentBookedConsumer>();

            configuration.UsingRabbitMq(
                (context, rabbitMqConfiguration) =>
                {
                    rabbitMqConfiguration.Host(
                        host,
                        "/",
                        hostConfiguration =>
                        {
                            hostConfiguration.Username(
                                username);

                            hostConfiguration.Password(
                                password);
                        });

                    rabbitMqConfiguration.ReceiveEndpoint(
                        "appointment-booked-queue",
                        endpointConfiguration =>
                        {
                            endpointConfiguration
                                .ConfigureConsumer<
                                    AppointmentBookedConsumer>(
                                        context);
                        });
                });
        });
}

static void ConfigureHostOptions(
    IServiceCollection services)
{
    services.Configure<HostOptions>(
        options =>
        {
            options.ShutdownTimeout =
                TimeSpan.FromSeconds(30);
        });
}

static void RegisterBackgroundServices(
    IServiceCollection services)
{
    services.AddHostedService<
        HeartbeatService>();

    services.AddHostedService<
        NotificationCleanupService>();

    services.AddHostedService<
        DoctorAvailabilityMonitorService>();
}

static void ConfigureSwagger(
    IServiceCollection services)
{
    services.AddEndpointsApiExplorer();

    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = "HealthAxis API",
                Version = "v1",
                Description =
                    "API for HealthAxis Healthcare System"
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
                Description =
                    "Enter 'Bearer {your token}'"
            });

        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference =
                            new OpenApiReference
                            {
                                Type =
                                    ReferenceType.SecurityScheme,

                                Id = "Bearer"
                            }
                    },
                    Array.Empty<string>()
                }
            });
    });
}

static void ConfigureCors(
    IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddPolicy(
            CorsPolicyName,
            policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:4200",
                        "https://localhost:7107")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
    });
}

static async Task SeedIdentityDataAsync(
    IServiceProvider serviceProvider)
{
    using var scope =
        serviceProvider.CreateScope();

    var roleManager =
        scope.ServiceProvider
            .GetRequiredService<
                RoleManager<IdentityRole>>();

    var userManager =
        scope.ServiceProvider
            .GetRequiredService<
                UserManager<ApplicationUser>>();

    await RoleSeeder.SeedRolesAsync(
        roleManager);

    await AdminSeeder.SeedAdminAsync(
        userManager);
}

static void ConfigureHttpPipeline(
    WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging(
        options =>
        {
            options.MessageTemplate =
                "HTTP {RequestMethod} {RequestPath} " +
                "responded {StatusCode} in " +
                "{Elapsed:0.0000} ms";

            options.GetLevel =
                (httpContext, _, exception) =>
                {
                    if (
                        exception != null ||
                        httpContext.Response.StatusCode >= 500
                    )
                    {
                        return LogEventLevel.Error;
                    }

                    if (
                        httpContext.Response.StatusCode >= 400
                    )
                    {
                        return LogEventLevel.Warning;
                    }

                    return LogEventLevel.Information;
                };
        });

    app.UseMiddleware<
        GlobalExceptionHandler>();

    app.UseCors(CorsPolicyName);

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}
