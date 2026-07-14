using HealthApp.API.BackgroundServices;
using HealthApp.API.Data;
using HealthApp.API.Exceptions;
using HealthApp.API.Identity;
using HealthApp.API.Mappings;
using HealthApp.API.Messaging;
using HealthApp.API.Options;
using HealthApp.API.Repository.Impl;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.API.Service.Interface;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
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
                Title = "HealthApp API",
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

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddDbContext<HealthAppDbContext>(options =>
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "HealthDbConnection"));
    });

    builder.Services
        .AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<HealthAppDbContext>()
        .AddSignInManager<SignInManager<ApplicationUser>>()
        .AddDefaultTokenProviders();

    var jwt = builder.Configuration.GetSection("Jwt");

    var jwtKey = jwt["Key"]
        ?? throw new InvalidOperationException("Jwt:Key missing");

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
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

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

                    ClockSkew = TimeSpan.Zero,

                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.NameIdentifier
                };
        });

    builder.Services.AddAuthorization();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAdminBlazor", policy =>
        {
            policy
                .WithOrigins(
                    "https://localhost:7083",
                    "http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    builder.Services.AddHttpContextAccessor();

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

    builder.Services.AddScoped<
        IRefreshTokenRepository,
        RefreshTokenRepository>();

    builder.Services.AddScoped<
        IDoctorLeaveRepository,
        DoctorLeaveRepository>();

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
        IDoctorLeaveService,
        DoctorLeaveService>();

    var rabbitmqConfig =
        builder.Configuration.GetSection("RabbitMq");

    builder.Services.AddMassTransit(configuration =>
    {
        configuration.AddConsumer<AppointmentBookedConsumer>();

        configuration.UsingRabbitMq((context, rabbitMq) =>
        {
            rabbitMq.Host(
                rabbitmqConfig["HostName"],
                rabbitmqConfig["VirtualHost"],
                host =>
                {
                    host.Username(
                        rabbitmqConfig["UserName"]!);

                    host.Password(
                        rabbitmqConfig["Password"]!);
                });

            rabbitMq.ReceiveEndpoint(
                rabbitmqConfig["AppointmentQueue"]!,
                endpoint =>
                {
                    endpoint.ConfigureConsumer<
                        AppointmentBookedConsumer>(context);
                });
        });
    });

    builder.Services.AddAutoMapper(configuration =>
    {
        configuration.AddProfile<MappingProfile>();
    });

    builder.Services.Configure<GarnetOptions>(
        builder.Configuration.GetSection("Garnet"));

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        var garnetOptions = builder.Configuration
            .GetSection("Garnet")
            .Get<GarnetOptions>()
            ?? new GarnetOptions();

        options.Configuration =
            garnetOptions.ConnectionString;

        options.InstanceName =
            garnetOptions.InstanceName;
    });

    builder.Services.AddHostedService<HeartbeatService>();

    builder.Services.AddHostedService<
        NotificationCleanupService>();

    var app = builder.Build();

    app.UseExceptionHandler();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors("AllowAdminBlazor");

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded " +
            "{StatusCode} in {Elapsed:0.0000} ms";

        options.EnrichDiagnosticContext =
            (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set(
                    "RequestHost",
                    httpContext.Request.Host.Value
                    ?? string.Empty);

                diagnosticContext.Set(
                    "RequestScheme",
                    httpContext.Request.Scheme);

                diagnosticContext.Set(
                    "UserName",
                    httpContext.User.Identity?.Name
                    ?? "Anonymous");
            };
    });

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var logger =
            services.GetRequiredService<ILogger<Program>>();

        try
        {
            var dbContext =
                services.GetRequiredService<HealthAppDbContext>();

            await dbContext.Database.MigrateAsync();

            var roleManager =
                services.GetRequiredService<
                    RoleManager<IdentityRole>>();

            var userManager =
                services.GetRequiredService<
                    UserManager<ApplicationUser>>();

            await RoleSeeder.SeedRolesAsync(roleManager);

            await RoleSeeder.SeedAdminAsync(userManager);

            logger.LogInformation(
                "Database migration and identity seeding " +
                "completed successfully.");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "An error occurred while applying database " +
                "migrations or seeding identity data during " +
                "application startup.",
                ex);
        }
    }

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(
        ex,
        "HealthApp API terminated unexpectedly.");
}
finally
{
    await Log.CloseAndFlushAsync();
}