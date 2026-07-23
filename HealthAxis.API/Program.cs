using HealthAxis.API.BackgroundServices;
using HealthAxis.API.Consumers;
using HealthAxis.API.Data;
using HealthAxis.API.Mappings;
using HealthAxis.API.Messaging;
using HealthAxis.API.Middlewares;
using HealthAxis.API.Repositories.Implementations;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services;
using HealthAxis.API.Services.Implementation;
using HealthAxis.API.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(
        (context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services);
        });

    builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy =
                JsonNamingPolicy.CamelCase;

            options.JsonSerializerOptions.Converters.Add(
                new JsonStringEnumConverter());
        });

    builder.Services.AddEndpointsApiExplorer();

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

    var databaseConnection =
        builder.Configuration.GetConnectionString("DbConnection");

    if (string.IsNullOrWhiteSpace(databaseConnection))
    {
        throw new InvalidOperationException(
            "The DbConnection connection string is missing.");
    }

    builder.Services.AddDbContext<ApplicationDbContext>(
        options =>
        {
            options.UseSqlServer(
                databaseConnection,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay:
                            TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
        });

    builder.Services.AddIdentity<
        IdentityUser,
        IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
        })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            return Task.CompletedTask;
        };

        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode =
                StatusCodes.Status403Forbidden;

            return Task.CompletedTask;
        };
    });

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
        var jwtSection =
            builder.Configuration.GetSection("Jwt");

        var jwtKey = jwtSection["Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException(
                "The JWT signing key is missing.");
        }

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSection["Issuer"],

                ValidateAudience = true,
                ValidAudience = jwtSection["Audience"],

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),

                ClockSkew = TimeSpan.Zero
            };
    });

    builder.Services.AddAuthorization();

    builder.Services.AddOpenApi();

    builder.Services.AddScoped<
        IAdminService,
        AdminService>();

    builder.Services.AddScoped(
        typeof(IRepository<>),
        typeof(Repository<>));

    builder.Services.AddScoped<
        IPatientRepository,
        PatientRepository>();

    builder.Services.AddScoped<
        IPatientService,
        PatientService>();

    builder.Services.AddScoped<
        IDoctorRepository,
        DoctorRepository>();

    builder.Services.AddScoped<
        IDoctorService,
        DoctorService>();

    builder.Services.AddScoped<
        IAppointmentRepository,
        AppointmentRepository>();

    builder.Services.AddScoped<
        IAppointmentService,
        AppointmentService>();

    builder.Services.AddScoped<
        IHealthRecordRepository,
        HealthRecordRepository>();

    builder.Services.AddScoped<
        IHealthRecordService,
        HealthRecordService>();

    builder.Services.AddScoped<
        INotificationService,
        NotificationService>();

    builder.Services.AddScoped<
        IAuthService,
        AuthService>();

    builder.Services.AddScoped<
        IEventPublisher,
        MassTransitEventPublisher>();

    builder.Services.AddMassTransit(configuration =>
    {
        configuration.AddConsumer<
            AppointmentBookedConsumer>();

        configuration.UsingRabbitMq(
            (context, rabbitMqConfig) =>
            {
                var rabbitMqSection = context
                    .GetRequiredService<IConfiguration>()
                    .GetSection("RabbitMq");

                var hostName =
                    rabbitMqSection["HostName"]
                    ?? "localhost";

                var virtualHost =
                    rabbitMqSection["VirtualHost"]
                    ?? "/";

                var userName =
                    rabbitMqSection["UserName"]
                    ?? "guest";

                var password =
                    rabbitMqSection["Password"]
                    ?? "guest";

                var queueName =
                    rabbitMqSection[
                        "AppointmentBookedQueue"]
                    ?? "healthaxis.appointment.booked.queue";

                rabbitMqConfig.Host(
                    hostName,
                    virtualHost,
                    host =>
                    {
                        host.Username(userName);
                        host.Password(password);
                    });

                rabbitMqConfig.ReceiveEndpoint(
                    queueName,
                    endpoint =>
                    {
                        endpoint.UseMessageRetry(
                            retry =>
                                retry.Interval(
                                    3,
                                    TimeSpan.FromSeconds(5)));

                        endpoint.ConfigureConsumer<
                            AppointmentBookedConsumer>(
                                context);
                    });
            });
    });

    builder.Services.AddHostedService<
        HeartbeatService>();

    builder.Services.AddHostedService<
        NotificationCleanupService>();

    builder.Services.AddAutoMapper(configuration =>
    {
        configuration.AddProfile<MappingProfile>();
    });

    builder.Services.AddExceptionHandler<
        GlobalExceptionHandler>();

    builder.Services.AddProblemDetails();

    const string corsPolicyName =
        "AllowHealthAxisClients";

    var localOrigins = new[]
    {
        "http://localhost:4200",
        "https://localhost:4200",
        "http://localhost:58189",
        "https://localhost:58189",
        "https://localhost:7172"
    };

    var deployedOrigins = builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>()
        ?? [];

    var allowedOrigins = localOrigins
        .Concat(deployedOrigins)
        .Where(origin =>
            !string.IsNullOrWhiteSpace(origin))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            corsPolicyName,
            policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    var app = builder.Build();

    app.UseExceptionHandler();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    var contentTypeProvider =
        new FileExtensionContentTypeProvider();

    contentTypeProvider.Mappings[".dat"] =
        "application/octet-stream";

    contentTypeProvider.Mappings[".wasm"] =
        "application/wasm";

    contentTypeProvider.Mappings[".dll"] =
        "application/octet-stream";

    contentTypeProvider.Mappings[".blat"] =
        "application/octet-stream";

    app.UseStaticFiles(
        new StaticFileOptions
        {
            ContentTypeProvider =
                contentTypeProvider
        });

    app.UseCors(corsPolicyName);

    app.UseSerilogRequestLogging();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // Open Angular when only the API root URL is requested.
    app.MapGet("/", () =>
        Results.Redirect("/angular/"));

    // Serve Angular directly. There is intentionally no redirect here,
    // which prevents the trailing-slash redirect loop.
    app.MapGet("/angular", async context =>
    {
        await SendSpaIndexAsync(
            context,
            app.Environment.WebRootPath,
            "angular");
    });

    // Support Angular client-side routes such as:
    // /angular/login and /angular/patient/dashboard
    app.MapFallbackToFile(
        "/angular/{*path:nonfile}",
        "angular/index.html");

    // Serve Blazor directly. There is intentionally no redirect here.
    app.MapGet("/blazor", async context =>
    {
        await SendSpaIndexAsync(
            context,
            app.Environment.WebRootPath,
            "blazor");
    });

    // Support Blazor client-side routes such as:
    // /blazor/external-login and /blazor/dashboard
    app.MapFallbackToFile(
        "/blazor/{*path:nonfile}",
        "blazor/index.html");

    using (var scope = app.Services.CreateScope())
    {
        var roleManager =
            scope.ServiceProvider.GetRequiredService<
                RoleManager<IdentityRole>>();

        var userManager =
            scope.ServiceProvider.GetRequiredService<
                UserManager<IdentityUser>>();

        await RoleSeeder.SeedRolesAsync(roleManager);

        await AdminSeeder.SeedAdminAsync(
            userManager,
            roleManager);
    }

    app.Lifetime.ApplicationStarted.Register(() =>
    {
        foreach (var url in app.Urls)
        {
            var baseUrl = url.TrimEnd('/');

            Log.Information(
                "HealthAxis API: {Url}",
                baseUrl);

            Log.Information(
                "Angular Portal: {Url}",
                $"{baseUrl}/angular/");

            Log.Information(
                "Blazor Admin: {Url}",
                $"{baseUrl}/blazor/");
        }
    });

    Log.Information(
        "HealthAxis API started successfully.");

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
    await Log.CloseAndFlushAsync();
}

static async Task SendSpaIndexAsync(
    HttpContext context,
    string webRootPath,
    string applicationFolder)
{
    var indexPath = Path.Combine(
        webRootPath,
        applicationFolder,
        "index.html");

    if (!File.Exists(indexPath))
    {
        context.Response.StatusCode =
            StatusCodes.Status404NotFound;

        await context.Response.WriteAsync(
            $"{applicationFolder}/index.html was not found.");

        return;
    }

    context.Response.ContentType =
        "text/html; charset=utf-8";

    await context.Response.SendFileAsync(indexPath);
}