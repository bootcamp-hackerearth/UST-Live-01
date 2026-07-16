using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using HealthApp.Api.BackgroundServices;
using HealthApp.Api.Data;
using HealthApp.Api.Mappings;
using HealthApp.Api.Messaging.Publisher;
using HealthApp.Api.Middleware;
using HealthApp.Api.Options;
using HealthApp.Api.Repository.Impl;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Impl;
using HealthApp.Api.Service.Interface;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using Serilog.Debugging;
using System.Text;
using System.Text.Json;


SelfLog.Enable(msg => Console.Error.WriteLine($"SERILOG ERROR: {msg}"));

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("HealthAxis API is starting up...");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Elasticsearch(
            new[] { new Uri("http://localhost:9200") },
            opts =>
            {
                opts.DataStream = new DataStreamName("logs", "healthaxis", "api");
                opts.BootstrapMethod = BootstrapMethod.Failure;
            });
});



builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddDbContext<HealthAppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;

    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
})
.AddEntityFrameworkStores<HealthAppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");

        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["key"]!)),

            ClockSkew = TimeSpan.Zero
        };
    });

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
        Description = "Enter JWT token only. Do not type Bearer manually."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

var angularUrl = builder.Configuration["AppUrls:AngularUrl"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins(angularUrl!)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();

builder.Services.AddScoped<IAppointmentEventPublisher, AppointmentEventPublisher>();
builder.Services.AddScoped<IDoctorLeaveRepository, DoctorLeaveRepository>();
builder.Services.AddScoped<IDoctorLeaveService, DoctorLeaveService>();




builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqOptions = builder.Configuration
            .GetSection("RabbitMq")
            .Get<RabbitMqOptions>() ?? new RabbitMqOptions();

        cfg.Host(
            rabbitMqOptions.HostName,
            rabbitMqOptions.VirtualHost,
            h =>
            {
                h.Username(rabbitMqOptions.UserName);
                h.Password(rabbitMqOptions.Password);
            });
    });
});

builder.Services.Configure<GarnetOptions>(builder.Configuration.GetSection("Garnet"));
builder.Services.AddStackExchangeRedisCache(options =>
{
    var garnetoptions = builder.Configuration.GetSection("Garnet")
    .Get<GarnetOptions>() ?? new GarnetOptions();


    options.Configuration = garnetoptions.ConnectionString;
    options.InstanceName = garnetoptions.InstanceName;
});



builder.Services.AddHostedService<HeartbeatBackgroundService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.seedroleAsync(roleManager);
}


await app.RunAsync();