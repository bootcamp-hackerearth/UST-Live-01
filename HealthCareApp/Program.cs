using HealthCareApp.BackgroundServices;
using HealthCareApp.Consumers;
using HealthCareApp.Data;
using HealthCareApp.Mapping;
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
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction() &&
    string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
{
    builder.WebHost.UseUrls("http://+:5000");
}

#region Serilog

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

#endregion

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AppointmentBookedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost =
            builder.Configuration["RabbitMQ:Host"] ?? "localhost";

        var rabbitMqPort =
            builder.Configuration.GetValue<ushort>("RabbitMQ:Port", 5672);

        cfg.Host(
            rabbitMqHost,
            rabbitMqPort,
            "/",
            h =>
            {
                h.Username(
                    builder.Configuration["RabbitMQ:Username"]!);

                h.Password(
                    builder.Configuration["RabbitMQ:Password"]!);
            });

        cfg.ReceiveEndpoint("appointment-booked", endpoint =>
        {
            endpoint.UseMessageRetry(retry =>
            {
                retry.Interval(
                    3,
                    TimeSpan.FromSeconds(5));
            });

            endpoint.ConfigureConsumer<AppointmentBookedConsumer>(context);
        });
    });
});

builder.Services.Configure<NotificationCleanupOptions>(
    builder.Configuration.GetSection("NotificationCleanup"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddDistributedMemoryCache();

builder.Services.AddDbContext<HealthAxisDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DbCon")));

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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HealthApp API",
        Version = "v1"
    });

    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter JWT token only. Do not type Bearer."
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("bearer", document)] = []
        });
});

builder.Services.AddScoped<DbContext, HealthAxisDbContext>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
builder.Services.AddScoped<ICacheService, NoOpCacheService>();
builder.Services.AddScoped<IDoctorLeaveService, DoctorLeaveService>();
builder.Services.AddScoped<IPatientNotificationService, PatientNotificationService>();

builder.Services.AddHostedService<HeartbeatBackgroundService>();
builder.Services.AddHostedService<NotificationCleanupBackgroundService>();
builder.Services.AddHostedService<OutboxPublisherBackgroundService>();

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
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

#region Seed Roles and Admin

var seedDataEnabled =
    builder.Configuration.GetValue("SeedData:Enabled", true);

var failStartupOnSeedError =
    builder.Configuration.GetValue("SeedData:FailStartupOnError", true);

if (seedDataEnabled)
{
    try
    {
        using var scope = app.Services.CreateScope();

        var roleManager =
            scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        await RoleSeeder.SeedRoleAsync(roleManager);

        await AdminSeeder.SeedAdminAsync(
            userManager,
            roleManager,
            builder.Configuration);

        Log.Information("Database role/admin seeding completed successfully.");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Database role/admin seeding failed during application startup.");

        if (failStartupOnSeedError)
        {
            throw;
        }
    }
}

#endregion

app.UseExceptionHandler();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/admin", out var remainingPath))
    {
        var targetPath = "/blazor/admin" + remainingPath;

        var queryString = context.Request.QueryString.HasValue
            ? context.Request.QueryString.Value
            : string.Empty;

        context.Response.Redirect(targetPath + queryString);

        return;
    }

    await next();
});

var contentTypeProvider = new FileExtensionContentTypeProvider();

contentTypeProvider.Mappings[".data"] = "application/octet-stream";
contentTypeProvider.Mappings[".wasm"] = "application/wasm";
contentTypeProvider.Mappings[".blat"] = "application/octet-stream";
contentTypeProvider.Mappings[".dll"] = "application/octet-stream";
contentTypeProvider.Mappings[".dat"] = "application/octet-stream";
contentTypeProvider.Mappings[".json"] = "application/json";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = contentTypeProvider
});

app.UseCors(ClientCorsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/angular"));

app.MapFallbackToFile(
    "/angular/{*path:nonfile}",
    "angular/index.html");

app.MapFallbackToFile(
    "/blazor/{*path:nonfile}",
    "blazor/index.html");

try
{
    await app.RunAsync();
}
finally
{
    await Log.CloseAndFlushAsync();
}