using HealthAxisCore_Api.BackgroundServices;
using HealthAxisCore_Api.Consumers;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Mappings;
using HealthAxisCore_Api.Middleware;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Options;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementation;
using HealthAxisCore_Api.Services.Implementations;
using HealthAxisCore_Api.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting HealthAxis API");

    builder.Services.AddControllers();

    builder.Services.AddDbContext<HealthAppDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
        ));

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<HealthAppDbContext>()
        .AddDefaultTokenProviders();

    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.Configure<GarnetOptions>(
    builder.Configuration.GetSection(GarnetOptions.SectionName));

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        var garnetOptions =
            builder.Configuration.GetSection(GarnetOptions.SectionName);

        options.Configuration =
            garnetOptions["ConnectionString"];

        options.InstanceName =
            garnetOptions["InstanceName"];
    });

    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<MappingProfile>();
    });

    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    builder.Services.AddScoped<IPatientRepository, PatientRepository>();
    builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
    builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
    builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

    builder.Services.AddScoped<IPatientService, PatientService>();
    builder.Services.AddScoped<IDoctorService, DoctorService>();
    builder.Services.AddScoped<IAppointmentService, AppointmentService>();
    builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();

    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ICacheService, CacheService>();

    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<AppointmentBookedConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");

            cfg.Host(rabbitMqSettings["Host"], "/", h =>
            {
                h.Username(rabbitMqSettings["Username"]!);
                h.Password(rabbitMqSettings["Password"]!);
            });

            cfg.ReceiveEndpoint("appointment-booked-queue", e =>
            {
                e.ConfigureConsumer<AppointmentBookedConsumer>(context);
            });
        });
    });

    builder.Services.Configure<HostOptions>(options =>
    {
        options.ShutdownTimeout = TimeSpan.FromSeconds(30);
    });

    builder.Services.AddHostedService<HeartbeatService>();
    builder.Services.AddHostedService<NotificationCleanupService>();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "HealthAxis API",
            Version = "v1",
            Description = "API for HealthAxis Healthcare System"
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer {your token}'"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

    const string CorsPolicy = "CorsPolicy";

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(CorsPolicy, policy =>
        {
            policy.WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:7107"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await RoleSeeder.SeedRolesAsync(roleManager);
        await AdminSeeder.SeedAdminAsync(userManager);
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

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
    });

    app.UseMiddleware<GlobalExceptionHandler>();

    app.UseCors(CorsPolicy);

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "HealthAxis API terminated unexpectedly");
}
finally
{
    Log.Information("HealthAxis API stopped");
    Log.CloseAndFlush();
}