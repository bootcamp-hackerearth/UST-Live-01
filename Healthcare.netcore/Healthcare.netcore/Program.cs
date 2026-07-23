using AutoMapper;
using HealthAxis.API.BackgroundServices;
using HealthAxis.API.Consumers;
using HealthAxis.API.Data;
using HealthAxis.API.Middleware;
using HealthAxis.API.Models.Auth;
using HealthAxis.API.Repositories.Implementations;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementations;
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

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog(
        (services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(
                    builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });

    // Controllers
    builder.Services.AddControllers();

    // Hosted background services
    builder.Services.AddHostedService<HeartbeatService>();

    builder.Services.AddHostedService<
        NotificationCleanupService>();

    // Global exception handling
    builder.Services.AddExceptionHandler<
        GlobalExceptionHandler>();

    builder.Services.AddProblemDetails();

    // Swagger
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = "HealthAxis API",
                Version = "v1"
            });

        options.AddSecurityDefinition(
            "bearer",
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description =
                    "Enter JWT token as: Bearer {your token}"
            });

        options.AddSecurityRequirement(
            document =>
                new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecuritySchemeReference(
                            "bearer",
                            document)
                    ] = new List<string>()
                });
    });

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            "AllowAll",
            policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    // Database
    var connectionString =
        builder.Configuration.GetConnectionString(
            "DefaultConnection");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException(
            "The DefaultConnection connection string is missing.");
    }

    builder.Services.AddDbContext<
        HealthAxisDbContext>(
        options =>
            options.UseSqlServer(connectionString));

    // ASP.NET Core Identity
    builder.Services
        .AddIdentity<
            ApplicationUser,
            IdentityRole>()
        .AddEntityFrameworkStores<
            HealthAxisDbContext>()
        .AddDefaultTokenProviders();

    // JWT authentication
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
            "Jwt:Key is missing.");
    }

    builder.Services
        .AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
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
                            Encoding.UTF8.GetBytes(
                                jwtKey))
                };
        });

    builder.Services.AddAuthorization();

    // Generic repository
    builder.Services.AddScoped(
        typeof(IRepository<>),
        typeof(Repository<>));

    // Patient
    builder.Services.AddScoped<
        IPatientRepository,
        PatientRepository>();

    builder.Services.AddScoped<
        IPatientService,
        PatientService>();

    // Doctor
    builder.Services.AddScoped<
        IDoctorService,
        DoctorService>();

    // Appointment
    builder.Services.AddScoped<
        IAppointmentRepository,
        AppointmentRepository>();

    builder.Services.AddScoped<
        IAppointmentService,
        AppointmentService>();

    // Health record
    builder.Services.AddScoped<
        IHealthRecordRepository,
        HealthRecordRepository>();

    builder.Services.AddScoped<
        IHealthRecordService,
        HealthRecordService>();

    // Authentication service
    builder.Services.AddScoped<
        IAuthService,
        AuthService>();

    // AutoMapper
    builder.Services.AddAutoMapper(
        configuration =>
        {
        },
        AppDomain.CurrentDomain.GetAssemblies());

    

    // MassTransit and RabbitMQ remain enabled.
    builder.Services.AddMassTransit(configuration =>
    {
        configuration.AddConsumer<
            AppointmentBookedConsumer>();

        configuration.UsingRabbitMq(
            (context, rabbitMqConfiguration) =>
            {
                var rabbitConfig =
                    builder.Configuration.GetSection(
                        "RabbitMq");

                var hostName =
                    rabbitConfig["HostName"];

                var virtualHost =
                    rabbitConfig["VirtualHost"];

                var userName =
                    rabbitConfig["UserName"];

                var password =
                    rabbitConfig["Password"];

                var appointmentQueue =
                    rabbitConfig["AppointmentQueue"];

                if (string.IsNullOrWhiteSpace(hostName))
                {
                    throw new InvalidOperationException(
                        "RabbitMq:HostName is missing.");
                }

                if (string.IsNullOrWhiteSpace(virtualHost))
                {
                    throw new InvalidOperationException(
                        "RabbitMq:VirtualHost is missing.");
                }

                if (string.IsNullOrWhiteSpace(userName))
                {
                    throw new InvalidOperationException(
                        "RabbitMq:UserName is missing.");
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new InvalidOperationException(
                        "RabbitMq:Password is missing.");
                }

                if (
                    string.IsNullOrWhiteSpace(
                        appointmentQueue))
                {
                    throw new InvalidOperationException(
                        "RabbitMq:AppointmentQueue is missing.");
                }

                rabbitMqConfiguration.Host(
                    hostName,
                    virtualHost,
                    hostConfiguration =>
                    {
                        hostConfiguration.Username(
                            userName);

                        hostConfiguration.Password(
                            password);
                    });

                rabbitMqConfiguration.ReceiveEndpoint(
                    appointmentQueue,
                    endpoint =>
                    {
                        endpoint.ConfigureConsumer<
                            AppointmentBookedConsumer>(
                            context);
                    });
            });
    });

    var app = builder.Build();

    // Structured HTTP request logging
    app.UseSerilogRequestLogging();

    // CORS
    app.UseCors("AllowAll");

    // Swagger is currently enabled only locally.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    var contentTypeProvider = new FileExtensionContentTypeProvider();
    contentTypeProvider.Mappings[".dat"] = "application/octet-stream";
    contentTypeProvider.Mappings[".wasm"] = "application/wasm";
    app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = contentTypeProvider });
    // Global exception handling
    app.UseExceptionHandler();

    // Authentication and authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // API controllers
    app.MapControllers();
    app.MapGet("/angular", async context =>
    {
        await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "angular", "index.html"));
    });
    app.MapGet("/angular/{*path:nonfile}", async context =>
    {
        await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "angular", "index.html"));
    });
    app.MapGet("/blazor", async context =>
    {
        await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "blazor", "index.html"));
    });
    app.MapGet("/blazor/{*path:nonfile}", async context =>
    {
        await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "blazor", "index.html"));
    });


    // Seed roles and Admin account
    using (var scope = app.Services.CreateScope())
    {
        var roleManager =
            scope.ServiceProvider
                .GetRequiredService<
                    RoleManager<IdentityRole>>();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<
                    UserManager<ApplicationUser>>();

        await RoleSeeder.SeedRoles(roleManager);

        await AdminSeeder.SeedAdmin(userManager);
    }

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