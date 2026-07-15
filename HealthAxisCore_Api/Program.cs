using HealthAxisCore_Api.BackgroundServices;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Mappings;
using HealthAxisCore_Api.Messaging.Consumers;
using HealthAxisCore_Api.Middleware;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Options;
using HealthAxisCore_Api.Repositories.Implementation;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Implementation;
using HealthAxisCore_Api.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

try
{
    Log.Information("[APP-BOOTSTRAP] Bootstrapping HealthAxis API");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });

    builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
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
                Description = "Healthcare Appointment Management API"
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
                Description = "Enter JWT token.\n\nExample: Bearer eyJhbGciOiJIUzI1NiIs..."
            });

        options.AddSecurityRequirement(document =>
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(
                    "bearer",
                    document)] = []
            });
    });

    builder.Services.AddOpenApi();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
    });

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
        .AddSignInManager<SignInManager<ApplicationUser>>()
        .AddDefaultTokenProviders();

    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwt = builder.Configuration.GetSection("Jwt");

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

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

                ClockSkew = TimeSpan.Zero,

                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.NameIdentifier
            };
        });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontendClient", policy =>
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

    builder.Services.AddMemoryCache();

    builder.Services.Configure<GarnetOptions>(
        builder.Configuration.GetSection("Garnet"));

    builder.Services.AddSingleton<GarnetHostedService>();
    builder.Services.AddHostedService(sp =>
        sp.GetRequiredService<GarnetHostedService>());

    builder.Services.AddStackExchangeRedisCache(option =>
    {
        var garnetOptions =
            builder.Configuration.GetSection("Garnet").Get<GarnetOptions>()
            ?? new GarnetOptions();

        option.Configuration = garnetOptions.ConnectionString;
        option.InstanceName = garnetOptions.InstanceName;
    });

    builder.Services.AddMassTransit(config =>
    {
        config.AddConsumer<AppointmentBookedConsumer>();

        config.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(
                builder.Configuration["RabbitMq:HostName"] ?? "localhost",
                builder.Configuration["RabbitMq:VirtualHost"] ?? "/",
                host =>
                {
                    host.Username(builder.Configuration["RabbitMq:UserName"] ?? "guest");
                    host.Password(builder.Configuration["RabbitMq:Password"] ?? "guest");
                });

            cfg.ReceiveEndpoint(
                builder.Configuration["RabbitMq:AppointmentBookedQueue"] ?? "appointment.booked.queue",
                endpoint =>
                {
                    endpoint.ConfigureConsumer<AppointmentBookedConsumer>(context);
                });
        });
    });

    builder.Services.AddScoped<IAdminHandoffService, AdminHandoffService>();
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<IPatientRepository, PatientRepository>();
    builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
    builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
    builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

    builder.Services.AddScoped<IJwtService, JwtService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IPatientService, PatientService>();
    builder.Services.AddScoped<IDoctorService, DoctorService>();
    builder.Services.AddScoped<IAppointmentService, AppointmentService>();
    builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
    builder.Services.AddScoped<IAdminService, AdminService>();

    builder.Services.AddHostedService<HealthAxisHeartbeatService>();
    builder.Services.AddHostedService<AppointmentAutoCancellationService>();

    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<MappingProfile>();
    });

    var app = builder.Build();

    app.UseExceptionHandler();

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "[HTTP] {RequestMethod} {RequestPath} -> {StatusCode} in {Elapsed:0.00} ms";

        options.GetLevel = (httpContext, elapsed, exception) =>
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

        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set(
                "UserId",
                httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous");
            diagnosticContext.Set(
                "UserRole",
                httpContext.User.FindFirstValue(ClaimTypes.Role) ?? "None");
        };
    });

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors("AllowFrontendClient");

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("[APP-START] Starting HealthAxis API application");

    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await RoleSeeder.SeedRolesAsync(roleManager);
        await RoleSeeder.SeedAdminAsync(userManager);
    }

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "[APP-STOP-ERROR] HealthAxis API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}