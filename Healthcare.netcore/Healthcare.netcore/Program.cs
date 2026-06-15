using HealthAxis.API.Data;
using HealthAxis.API.Mappings;
using HealthAxis.API.Repositories.Impl;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Impl;
using HealthAxis.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Connection
builder.Services.AddDbContext<HealthAxisDbContext>(options =>
options.UseSqlServer(
builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repository Registration
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

// Service Registration
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();

var app = builder.Build();

// Configure HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
