using AutoMapper;
using HealthAxis.API.Data;
using HealthAxis.API.Middleware;
using HealthAxis.API.Models.Auth;
using HealthAxis.API.Repositories.Implementations;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementations;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using HealthAxis.API.BackgroundServices;
using Serilog;
using Serilog.Events;
using HealthAxis.API.Options;

using HealthAxis.API.Consumers;
using MassTransit;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSerilog((services, configuration) =>
    configuration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());


// ✅ Controllers
builder.Services.AddControllers();
//heartbeatservices
builder.Services.AddHostedService<HeartbeatService>();
builder.Services.AddHostedService<NotificationCleanupService>();



// ✅ Global Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ✅ Swagger + JWT Auth
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
        Description = "Enter JWT token as: Bearer {your token}"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = new List<string>()
    });
});

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// ✅ DbContext
builder.Services.AddDbContext<HealthAxisDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<HealthAxisDbContext>()
    .AddDefaultTokenProviders();

// ✅ JWT Auth
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
                Encoding.UTF8.GetBytes(jwt["Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();

// ✅ Generic Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// ✅ Patient
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

// ✅ Doctor
// ✅ Doctor
builder.Services.AddScoped<IDoctorService, DoctorService>();


// ✅ Appointment
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// ✅ HealthRecord
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();

// ✅ Auth
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddCors(p =>
{
    p.AddPolicy("CorsPolicy", cfg =>
    {
        cfg.WithOrigins("https://localhost:7273")

        .AllowAnyHeader().AllowAnyMethod();
    });
});

// ✅ AutoMapper
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

// ✅ MassTransit + RabbitMQ
builder.Services.AddMassTransit(x =>
{
x.AddConsumer<AppointmentBookedConsumer>();

x.UsingRabbitMq((context, cfg) =>
{
var rabbitConfig = builder.Configuration.GetSection("RabbitMq");

cfg.Host(
    rabbitConfig["HostName"],
    rabbitConfig["VirtualHost"],
    h =>
    {

        h.Username(rabbitConfig["UserName"]!);
        h.Password(rabbitConfig["Password"]!);
    });

    cfg.ReceiveEndpoint(rabbitConfig["AppointmentQueue"]!, e =>
    {
        e.ConfigureConsumer<AppointmentBookedConsumer>(context);
    });
});
});
builder.Services.Configure<GarnetOptions>(
    builder.Configuration.GetSection("Garnet"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    var garnetOptions = builder.Configuration
        .GetSection("Garnet")
        .Get<GarnetOptions>() ?? new GarnetOptions();

    options.Configuration = garnetOptions.ConnectionString;
    options.InstanceName = garnetOptions.InstanceName;
});

var app = builder.Build();
app.UseSerilogRequestLogging();


app.UseCors("AllowAll");

// ✅ Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ Global Exception Handler
app.UseExceptionHandler();

// ✅ Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ✅ Role Seeder
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    await RoleSeeder.SeedRoles(roleManager);
    await AdminSeeder.SeedAdmin(userManager);
}

app.Run();