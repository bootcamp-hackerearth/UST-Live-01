using HealthAxisCore_Api.BackgroundServices;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Mappings;
using HealthAxisCore_Api.Messaging.Consumers;
using HealthAxisCore_Api.Middleware;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories.Implementation;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Implementation;
using HealthAxisCore_Api.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using Serilog.Events;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] " +
        "{Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

try
{
    Log.Information(
        "[APP-BOOTSTRAP] Bootstrapping HealthAxis API");

    var builder = WebApplication.CreateBuilder(args);

    // ============================================================
    // Serilog
    // ============================================================

    builder.Host.UseSerilog(
        (context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });

    // ============================================================
    // Controllers and JSON
    // ============================================================

    builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy =
                JsonNamingPolicy.CamelCase;
        });

    builder.Services.AddEndpointsApiExplorer();

    // ============================================================
    // Swagger and OpenAPI
    // ============================================================

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = "HealthAxis API",
                Version = "v1",
                Description =
                    "Healthcare Appointment Management API"
            });

        options.AddSecurityDefinition(
            "bearer",
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description =
                    "Enter the JWT access token.\n\n" +
                    "Example: Bearer eyJhbGciOiJIUzI1NiIs..."
            });

        options.AddSecurityRequirement(document =>
            new OpenApiSecurityRequirement
            {
                [
                    new OpenApiSecuritySchemeReference(
                        "bearer",
                        document)
                ] = []
            });
    });

    builder.Services.AddOpenApi();

    // ============================================================
    // Global exception handling
    // ============================================================

    builder.Services
        .AddExceptionHandler<GlobalExceptionHandler>();

    builder.Services.AddProblemDetails();

    // ============================================================
    // SQL Server and Entity Framework Core
    // ============================================================

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var connectionString =
            builder.Configuration.GetConnectionString(
                "DbConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "The DbConnection connection string is missing. " +
                "Configure ConnectionStrings__DbConnection in " +
                "Elastic Beanstalk environment properties.");
        }

        options.UseSqlServer(
            connectionString,
            sqlServerOptions =>
            {
                sqlServerOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            });
    });

    // ============================================================
    // ASP.NET Core Identity
    // ============================================================

    builder.Services
        .AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;

            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddSignInManager<
            SignInManager<ApplicationUser>>()
        .AddDefaultTokenProviders();

    // ============================================================
    // JWT authentication
    // ============================================================

    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultForbidScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultScheme =
                JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtSection =
                builder.Configuration.GetSection("Jwt");

            var jwtIssuer = jwtSection["Issuer"];
            var jwtAudience = jwtSection["Audience"];
            var jwtKey = jwtSection["Key"];

            if (string.IsNullOrWhiteSpace(jwtIssuer))
            {
                throw new InvalidOperationException(
                    "Jwt:Issuer is missing.");
            }

            if (string.IsNullOrWhiteSpace(jwtAudience))
            {
                throw new InvalidOperationException(
                    "Jwt:Audience is missing.");
            }

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException(
                    "Jwt:Key is missing. Configure Jwt__Key in " +
                    "Elastic Beanstalk environment properties.");
            }

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,

                    ValidateAudience = true,
                    ValidAudience = jwtAudience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey)),

                    ClockSkew = TimeSpan.Zero,

                    RoleClaimType = ClaimTypes.Role,

                    NameClaimType =
                        ClaimTypes.NameIdentifier
                };
        });

    // ============================================================
    // CORS
    // ============================================================

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            "AllowFrontendClient",
            policy =>
            {
                policy
                    .WithOrigins(
                        "https://localhost:7050",
                        "https://localhost:4200",
                        "http://localhost:4200",
                        "http://localhost:56902")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    builder.Services.AddAuthorization();

    // ============================================================
    // Caching
    // ============================================================

    builder.Services.AddMemoryCache();

    // CHANGED:
    // DoctorService depends on IDistributedCache.
    //
    // Redis/Garnet is currently disabled, so this registers
    // MemoryDistributedCache as the IDistributedCache implementation.
    //
    // This registration must appear before builder.Build().
    builder.Services.AddDistributedMemoryCache(options =>
    {
        options.SizeLimit = null;
    });

    // ============================================================
    // MassTransit and RabbitMQ
    // ============================================================

    builder.Services.AddMassTransit(configuration =>
    {
        configuration.AddConsumer<
            AppointmentBookedConsumer>();

        configuration.UsingRabbitMq(
            (context, rabbitMqConfiguration) =>
            {
                var rabbitMqHost =
                    builder.Configuration[
                        "RabbitMq:HostName"]
                    ?? "localhost";

                var rabbitMqVirtualHost =
                    builder.Configuration[
                        "RabbitMq:VirtualHost"]
                    ?? "/";

                var rabbitMqUserName =
                    builder.Configuration[
                        "RabbitMq:UserName"]
                    ?? "guest";

                var rabbitMqPassword =
                    builder.Configuration[
                        "RabbitMq:Password"]
                    ?? "guest";

                rabbitMqConfiguration.Host(
                    rabbitMqHost,
                    rabbitMqVirtualHost,
                    host =>
                    {
                        host.Username(rabbitMqUserName);
                        host.Password(rabbitMqPassword);
                    });

                var appointmentQueue =
                    builder.Configuration[
                        "RabbitMq:AppointmentBookedQueue"]
                    ?? "appointment.booked.queue";

                rabbitMqConfiguration.ReceiveEndpoint(
                    appointmentQueue,
                    endpoint =>
                    {
                        endpoint.UseMessageRetry(retry =>
                        {
                            retry.Interval(
                                retryCount: 3,
                                interval:
                                    TimeSpan.FromSeconds(15));
                        });

                        endpoint.ConfigureConsumer<
                            AppointmentBookedConsumer>(
                                context);
                    });
            });
    });

    // ============================================================
    // Repositories
    // ============================================================

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

    // ============================================================
    // Application services
    // ============================================================

    builder.Services.AddScoped<
        IJwtService,
        JwtService>();

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

    builder.Services.AddScoped<
        IAdminHandoffService,
        AdminHandoffService>();

    // ============================================================
    // Sprint 4 background services
    // ============================================================

    builder.Services.AddHostedService<
        HealthAxisHeartbeatService>();

    builder.Services.AddHostedService<
        AppointmentAutoCancellationService>();

    // ============================================================
    // AutoMapper
    // ============================================================

    builder.Services.AddAutoMapper(configuration =>
    {
        configuration.AddProfile<MappingProfile>();
    });

    // ============================================================
    // Build application
    // ============================================================

    var app = builder.Build();

    // ============================================================
    // Verify frontend deployment files
    // ============================================================

    var webRootPath =
        app.Environment.WebRootPath
        ?? Path.Combine(
            app.Environment.ContentRootPath,
            "wwwroot");

    var angularIndexPath =
        Path.Combine(
            webRootPath,
            "angular",
            "index.html");

    var blazorIndexPath =
        Path.Combine(
            webRootPath,
            "blazor",
            "index.html");

    if (File.Exists(angularIndexPath))
    {
        Log.Information(
            "[ANGULAR-FILES-FOUND] " +
            "Angular index file found at {AngularIndexPath}",
            angularIndexPath);
    }
    else
    {
        Log.Error(
            "[ANGULAR-FILES-MISSING] " +
            "Angular index file was not found at {AngularIndexPath}. " +
            "Run the Angular production build before publishing.",
            angularIndexPath);
    }

    if (File.Exists(blazorIndexPath))
    {
        Log.Information(
            "[BLAZOR-FILES-FOUND] " +
            "Blazor index file found at {BlazorIndexPath}",
            blazorIndexPath);
    }
    else
    {
        Log.Warning(
            "[BLAZOR-FILES-MISSING] " +
            "Blazor index file was not found at {BlazorIndexPath}.",
            blazorIndexPath);
    }

    // ============================================================
    // Exception handling and HTTP request logging
    // ============================================================

    app.UseExceptionHandler();

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "[HTTP] {RequestMethod} {RequestPath} " +
            "-> {StatusCode} in {Elapsed:0.00} ms";

        options.GetLevel =
            (httpContext, elapsed, exception) =>
            {
                if (exception != null)
                {
                    return LogEventLevel.Error;
                }

                if (httpContext.Response.StatusCode >= 500)
                {
                    return LogEventLevel.Error;
                }

                if (httpContext.Response.StatusCode >= 400)
                {
                    return LogEventLevel.Warning;
                }

                return LogEventLevel.Information;
            };

        options.EnrichDiagnosticContext =
            (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set(
                    "RequestHost",
                    httpContext.Request.Host.Value);

                diagnosticContext.Set(
                    "RequestScheme",
                    httpContext.Request.Scheme);

                diagnosticContext.Set(
                    "UserId",
                    httpContext.User.FindFirstValue(
                        ClaimTypes.NameIdentifier)
                    ?? "Anonymous");

                diagnosticContext.Set(
                    "UserRole",
                    httpContext.User.FindFirstValue(
                        ClaimTypes.Role)
                    ?? "None");
            };
    });

    // ============================================================
    // Development-only middleware
    // ============================================================

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();

        app.UseSwagger();

        app.UseSwaggerUI();

        app.UseHttpsRedirection();
    }

    // ============================================================
    // Static files
    // ============================================================

    var contentTypeProvider =
        new FileExtensionContentTypeProvider();

    contentTypeProvider.Mappings[".dat"] =
        "application/octet-stream";

    contentTypeProvider.Mappings[".wasm"] =
        "application/wasm";

    app.UseStaticFiles(
        new StaticFileOptions
        {
            ContentTypeProvider =
                contentTypeProvider
        });

    // ============================================================
    // Routing, CORS, authentication and authorization
    // ============================================================

    app.UseRouting();

    app.UseCors("AllowFrontendClient");

    app.UseAuthentication();

    app.UseAuthorization();

    // ============================================================
    // API endpoints
    // ============================================================

    app.MapControllers();

    app.MapGet(
        "/",
        () => Results.Redirect("/angular/"));

    app.MapGet(
        "/health",
        () => Results.Ok(
            new
            {
                status = "Healthy",
                application = "HealthAxisCore_Api",
                environment =
                    app.Environment.EnvironmentName,
                timestampUtc = DateTime.UtcNow
            }));

    // ============================================================
    // Angular client-side routing
    // ============================================================

    app.MapFallbackToFile(
        "/angular/{*path:nonfile}",
        "angular/index.html");

    // ============================================================
    // Blazor client-side routing
    // ============================================================

    app.MapFallbackToFile(
        "/blazor/{*path:nonfile}",
        "blazor/index.html");

    // ============================================================
    // Identity role and admin seeding
    // ============================================================

    Log.Information(
        "[APP-START] Starting HealthAxis API application");

    try
    {
        using var scope =
            app.Services.CreateScope();

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

        await RoleSeeder.SeedAdminAsync(
            userManager);

        Log.Information(
            "[IDENTITY-SEED-SUCCESS] " +
            "Roles and admin user seeded successfully");
    }
    catch (Exception seedException)
    {
        Log.Error(
            seedException,
            "[IDENTITY-SEED-FAILED] " +
            "Role/admin seeding failed. The API will continue " +
            "starting, but login may fail until SQL Server " +
            "connectivity is restored.");
    }

    // ============================================================
    // Start application
    // ============================================================

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(
        ex,
        "[APP-STOP-ERROR] " +
        "HealthAxis API terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}