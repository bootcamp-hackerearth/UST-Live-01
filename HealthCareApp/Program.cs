using AutoMapper;
using HealthCareApp.BackgroundServices;
using HealthCareApp.Data;
using HealthCareApp.Mapping;
using HealthCareApp.Messaging.Consumers;
using HealthCareApp.Middleware;
using HealthCareApp.Options;
using HealthCareApp.Repository.Impl;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services;
using HealthCareApp.Services.Impl;
using HealthCareApp.Services.Interface;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Register Serilog.
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// Register embedded Garnet server for local development.
// This starts Garnet automatically when the backend API starts.
builder.Services.AddHostedService<GarnetHostedService>();

// Register Garnet/Redis options.
builder.Services.Configure<GarnetOptions>(
    builder.Configuration.GetSection("Garnet"));

// Register distributed cache using embedded Garnet.
builder.Services.AddStackExchangeRedisCache(options =>
{
    var garnetOptions = builder.Configuration
        .GetSection("Garnet")
        .Get<GarnetOptions>()!;

    options.Configuration = garnetOptions.ConnectionString;
    options.InstanceName = garnetOptions.InstanceName;
});

// Register HealthAxisDbContext with SQL Server.
builder.Services.AddDbContext<HealthAxisDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbCon")));

// Register ASP.NET Core Identity.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<HealthAxisDbContext>()
.AddDefaultTokenProviders();

// Register JWT Authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");

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

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HealthAxis API",
        Version = "v1"
    });

    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter JWT token only. Do not type Bearer."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

// Register DbContext for generic repository constructor.
builder.Services.AddScoped<DbContext, HealthAxisDbContext>();

// Register AutoMapper.
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

// Register generic repository.
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register entity-specific repositories.
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IDoctorLeaveRepository, DoctorLeaveRepository>();
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

// Register core services.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IDoctorLeaveService, DoctorLeaveService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Register DoctorService dependency wrapper to avoid too many constructor parameters.
builder.Services.AddScoped(serviceProvider => new DoctorServiceDependencies
{
    Repository = serviceProvider.GetRequiredService<IDoctorRepository>(),
    AppointmentRepository = serviceProvider.GetRequiredService<IAppointmentRepository>(),
    Mapper = serviceProvider.GetRequiredService<IMapper>(),
    UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>(),
    RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>(),
    CacheService = serviceProvider.GetRequiredService<ICacheService>(),
    DoctorLeaveService = serviceProvider.GetRequiredService<IDoctorLeaveService>(),
    Logger = serviceProvider.GetRequiredService<ILogger<DoctorService>>()
});

builder.Services.AddScoped<IDoctorService, DoctorService>();

// Register AppointmentService dependency wrapper to avoid too many constructor parameters.
builder.Services.AddScoped(serviceProvider => new AppointmentServiceDependencies
{
    AppointmentRepository = serviceProvider.GetRequiredService<IAppointmentRepository>(),
    PatientRepository = serviceProvider.GetRequiredService<IPatientRepository>(),
    DoctorRepository = serviceProvider.GetRequiredService<IDoctorRepository>(),
    HealthRecordRepository = serviceProvider.GetRequiredService<IHealthRecordRepository>(),
    DoctorLeaveService = serviceProvider.GetRequiredService<IDoctorLeaveService>(),
    Mapper = serviceProvider.GetRequiredService<IMapper>(),
    PublishEndpoint = serviceProvider.GetRequiredService<IPublishEndpoint>(),
    Logger = serviceProvider.GetRequiredService<ILogger<AppointmentService>>()
});

builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Register background services.
builder.Services.AddHostedService<HeartbeatBackgroundService>();
builder.Services.AddHostedService<NotificationCleanupService>();

// Register MassTransit with RabbitMQ.
builder.Services.AddMassTransit(configurator =>
{
    configurator.SetKebabCaseEndpointNameFormatter();

    configurator.AddConsumer<AppointmentBookedConsumer>();

    configurator.UsingRabbitMq((context, rabbitMqConfig) =>
    {
        var rabbitMqSection = builder.Configuration.GetSection("RabbitMq");

        rabbitMqConfig.Host(
            rabbitMqSection["Host"],
            rabbitMqSection["VirtualHost"],
            hostConfig =>
            {
                hostConfig.Username(rabbitMqSection["Username"]!);
                hostConfig.Password(rabbitMqSection["Password"]!);
            });

        rabbitMqConfig.ReceiveEndpoint(
            rabbitMqSection["AppointmentBookedQueue"]!,
            endpoint =>
            {

                endpoint.UseMessageRetry(retryConfig =>
                {
                    retryConfig.Interval(
                        retryCount: 3,
                        interval: TimeSpan.FromSeconds(5));
                });

                endpoint.ConfigureConsumer<AppointmentBookedConsumer>(context);
            });
    });
});

// Register Global Exception Handler.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

const string ClientCorsPolicy = "ClientCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(ClientCorsPolicy, policy =>
    {
        policy.WithOrigins(
                "https://localhost:7075",
                "http://localhost:4200",
                "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Seed roles and default admin.
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    await RoleSeeder.SeedRoleAsync(roleManager);

    await AdminSeeder.SeedAdminAsync(
        userManager,
        roleManager,
        builder.Configuration);
}

// Global exception handler middleware.
app.UseExceptionHandler();

// Serilog request logging middleware.
// This logs HTTP method, path, status code, and elapsed time.
// The diagnostic context fields can appear in Kibana as labels.StatusCode,
// labels.RequestMethod, labels.RequestPath, and labels.UserAgent.
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("StatusCode", httpContext.Response.StatusCode);
        diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
        diagnosticContext.Set("RequestPath", httpContext.Request.Path.Value ?? string.Empty);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
    };
});

// Configure HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(ClientCorsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

try
{
    await app.RunAsync();
}
finally
{
    await Log.CloseAndFlushAsync();
}