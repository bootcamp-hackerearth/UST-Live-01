using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;
using HealthAxisCore_Api.Services.Implementations;
using HealthAxisCore_Api.Mappings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add services to the container
builder.Services.AddControllers();

// ✅ Register DbContext with SQL Server
builder.Services.AddDbContext<HealthAppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// ✅ Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ✅ Register Generic Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// ✅ Register Entity-Specific Repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

// ✅ Register Services (VERY IMPORTANT)
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();

// ✅ Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// ✅ Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ✅ Authorization middleware
app.UseAuthorization();

// ✅ Map controllers
app.MapControllers();

app.Run();