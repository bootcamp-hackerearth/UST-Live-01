using HealthAxis.API.BackgroundServices;
using Microsoft.AspNetCore.StaticFiles;
using HealthAxis.API.Consumers;
using HealthAxis.API.Data;
using HealthAxis.API.Mappings;
using HealthAxis.API.Middleware;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

Log.Logger =
    new LoggerConfiguration()
        .WriteTo.Console()
        .CreateBootstrapLogger();

try
{
    Log.Information("Starting HealthAxis.API");

    WebApplicationBuilder builder =
        WebApplication.CreateBuilder(args);

    // Serilog configuration is read from appsettings.json.
    // Keep only the Console and File sinks in appsettings.json.
    builder.Services.AddSerilog((services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });

    // Background hosted services.
    builder.Services.AddHostedService<HealthAxisHeartbeatService>();
    builder.Services.AddHostedService<NotificationCleanupService>();

    // RabbitMQ and MassTransit.
    builder.Services.AddMassTransit(configuration =>
    {
        configuration.AddConsumer<AppointmentBookedConsumer>();

        configuration.UsingRabbitMq((context, rabbitMq) =>
        {
            IConfigurationSection rabbitMqSettings =
                builder.Configuration.GetSection("RabbitMq");

            string rabbitMqHost =
                rabbitMqSettings["Host"]
                ?? throw new InvalidOperationException(
                    "RabbitMQ host is missing.");

            string rabbitMqVirtualHost =
                rabbitMqSettings["VirtualHost"]
                ?? "/";

            string rabbitMqUsername =
                rabbitMqSettings["Username"]
                ?? throw new InvalidOperationException(
                    "RabbitMQ username is missing.");

            string rabbitMqPassword =
                rabbitMqSettings["Password"]
                ?? throw new InvalidOperationException(
                    "RabbitMQ password is missing.");

            rabbitMq.Host(
                rabbitMqHost,
                rabbitMqVirtualHost,
                host =>
                {
                    host.Username(rabbitMqUsername);
                    host.Password(rabbitMqPassword);
                });

            rabbitMq.ReceiveEndpoint(
                "appointment-booked-notification-queue",
                endpoint =>
                {
                    endpoint.ConfigureConsumer<AppointmentBookedConsumer>(
                        context);
                });
        });
    });

    // Controllers and JSON serialization.
    builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy =
                JsonNamingPolicy.CamelCase;
        });

    // Global exception handling.
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    // AutoMapper.
    builder.Services.AddAutoMapper(configuration =>
    {
        configuration.AddProfile<MappingProfile>();
    });

    // SQL Server DbContext.
    builder.Services.AddDbContext<HealthAxisDbContext>(options =>
    {
        string connectionString =
            builder.Configuration.GetConnectionString("HealthAxisDb")
            ?? throw new InvalidOperationException(
                "The HealthAxisDb connection string is missing.");

        options.UseSqlServer(connectionString);
    });

    // ASP.NET Core Identity.
    builder.Services
        .AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<HealthAxisDbContext>()
        .AddDefaultTokenProviders();

    // Prevent Identity from redirecting API requests to HTML login pages.
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

    // JWT authentication.
    builder.Services
        .AddAuthentication(options =>
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
                ?? throw new InvalidOperationException(
                    "JWT Key is missing.");

            string jwtIssuer =
                jwt["Issuer"]
                ?? throw new InvalidOperationException(
                    "JWT Issuer is missing.");

            string jwtAudience =
                jwt["Audience"]
                ?? throw new InvalidOperationException(
                    "JWT Audience is missing.");

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
            policy
                .WithOrigins(
                    "https://localhost:7051",
                    "http://localhost:5293",
                    "http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    // Swagger/OpenAPI.
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
                    "API for the HealthAxis Healthcare System"
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
                    "Paste only the JWT token. " +
                    "Do not type the word Bearer manually."
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

    // Generic repository.
    builder.Services.AddScoped(
        typeof(IRepository<>),
        typeof(Repository<>));

    // Repositories.
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

    // Application services.
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

    WebApplication app =
    builder.Build();

    // Global exception-handling middleware.
    app.UseExceptionHandler();

    // Swagger is enabled only in Development.
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

    // Structured HTTP request logging.
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded " +
            "{StatusCode} in {Elapsed:0.0000} ms";
    });

    // Local Development supports HTTP and HTTPS through launchSettings.
    // Elastic Beanstalk normally terminates HTTPS at its proxy.
    if (!app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    // Resolve the exact physical frontend entry files.
    string angularIndexPath =
        Path.Combine(
            app.Environment.WebRootPath,
            "angular",
            "index.html");

    string blazorIndexPath =
        Path.Combine(
            app.Environment.WebRootPath,
            "blazor",
            "index.html");

    // Log exactly which files the application will serve.
    Log.Information(
        "Web root path: {WebRootPath}",
        app.Environment.WebRootPath);

    Log.Information(
        "Angular index path: {AngularIndexPath}, Exists: {Exists}",
        angularIndexPath,
        File.Exists(angularIndexPath));

    Log.Information(
        "Blazor index path: {BlazorIndexPath}, Exists: {Exists}",
        blazorIndexPath,
        File.Exists(blazorIndexPath));

    if (File.Exists(blazorIndexPath))
    {
        string blazorIndexContent =
            File.ReadAllText(blazorIndexPath);

        Log.Information(
            "Blazor index has correct base path: {HasCorrectBase}",
            blazorIndexContent.Contains(
                "href=\"/blazor/\"",
                StringComparison.OrdinalIgnoreCase));
    }

    // Serve frontend index files without browser caching.
    static async Task SendFrontendIndexAsync(
        HttpContext context,
        string indexPath,
        string applicationName)
    {
        if (!File.Exists(indexPath))
        {
            context.Response.StatusCode =
                StatusCodes.Status404NotFound;

            await context.Response.WriteAsync(
                $"{applicationName} index.html was not found.");

            return;
        }

        context.Response.ContentType =
            "text/html; charset=utf-8";

        context.Response.Headers.CacheControl =
            "no-store, no-cache, must-revalidate";

        context.Response.Headers.Pragma =
            "no-cache";

        context.Response.Headers.Expires =
            "0";

        await context.Response.SendFileAsync(
            indexPath);
    }

    // Intercept Angular and Blazor client-side routes.
    //
    // Examples handled here:
    // /angular
    // /angular/
    // /angular/login
    // /blazor
    // /blazor/
    // /blazor/auth-callback
    // /blazor/dashboard
    //
    // Requests for physical files such as .css, .js, .wasm,
    // .json, .dll and .dat continue to UseStaticFiles.
    app.Use(async (context, next) =>
    {
        string requestPath =
            context.Request.Path.Value
            ?? string.Empty;

        bool isAngularPath =
            requestPath.Equals(
                "/angular",
                StringComparison.OrdinalIgnoreCase) ||
            requestPath.StartsWith(
                "/angular/",
                StringComparison.OrdinalIgnoreCase);

        bool isBlazorPath =
            requestPath.Equals(
                "/blazor",
                StringComparison.OrdinalIgnoreCase) ||
            requestPath.StartsWith(
                "/blazor/",
                StringComparison.OrdinalIgnoreCase);

        bool isPhysicalFileRequest =
            Path.HasExtension(requestPath);

        if (isAngularPath &&
            !isPhysicalFileRequest)
        {
            await SendFrontendIndexAsync(
                context,
                angularIndexPath,
                "Angular");

            return;
        }

        if (isBlazorPath &&
            !isPhysicalFileRequest)
        {
            await SendFrontendIndexAsync(
                context,
                blazorIndexPath,
                "Blazor");

            return;
        }

        await next();
    });

    // Configure content types required by Blazor WebAssembly.
    FileExtensionContentTypeProvider contentTypeProvider =
        new FileExtensionContentTypeProvider();

    contentTypeProvider.Mappings[".dat"] =
        "application/octet-stream";

    contentTypeProvider.Mappings[".wasm"] =
        "application/wasm";

    contentTypeProvider.Mappings[".dll"] =
        "application/octet-stream";

    contentTypeProvider.Mappings[".pdb"] =
        "application/octet-stream";

    // Serve physical files from wwwroot.
    app.UseStaticFiles(
        new StaticFileOptions
        {
            ContentTypeProvider =
                contentTypeProvider,

            OnPrepareResponse = context =>
            {
                string requestPath =
                    context.Context.Request.Path.Value
                    ?? string.Empty;

                // Do not cache frontend entry files during development.
                if (requestPath.EndsWith(
                        "/index.html",
                        StringComparison.OrdinalIgnoreCase))
                {
                    context.Context.Response.Headers.CacheControl =
                        "no-store, no-cache, must-revalidate";

                    context.Context.Response.Headers.Pragma =
                        "no-cache";

                    context.Context.Response.Headers.Expires =
                        "0";
                }
            }
        });

    app.UseCors("AllowFrontend");

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    // In Production, open Angular when the root URL is requested.
    // Swagger continues using the root URL during Development.
    if (!app.Environment.IsDevelopment())
    {
        app.MapGet(
            "/",
            async context =>
            {
                await SendFrontendIndexAsync(
                    context,
                    angularIndexPath,
                    "Angular");
            });
    }

    await app.RunAsync();
}
catch (Exception exception)
    when (exception is not HostAbortedException)
{
    Log.Fatal(
        exception,
        "HealthAxis.API terminated unexpectedly during startup");
}
finally
{
    Log.CloseAndFlush();
}