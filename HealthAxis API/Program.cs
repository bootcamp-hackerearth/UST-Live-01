using HealthAxis.API.Data;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Microsoft.EntityFrameworkCore;
using HealthAxis.API.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

builder.Services.AddDbContext<HealthAxisDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HealthAxisDb")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

var appName = builder.Configuration["AppSettings:AppName"] ?? "HealthAxis API";

app.Logger.LogInformation("{AppName} started successfully.", appName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Simple request/response logging middleware.
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    logger.LogInformation(
        "Incoming request: {Method} {Path}",
        context.Request.Method,
        context.Request.Path);

    await next();

    logger.LogInformation(
        "Outgoing response: {StatusCode}",
        context.Response.StatusCode);
});

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => new
{
    Application = appName,
    Status = "Running",
    Message = "Welcome to HealthAxis API"
});

app.Run();