using AutoMapper;
using HealthAxis.API.Data;
using HealthAxis.API.Middleware;
using HealthAxis.API.Repositories.Implementations;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementations;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi; // ✅ ONLY THIS
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ Controllers
builder.Services.AddControllers();

// ✅ Swagger (FIXED ✅🔥)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HealthAxis API",
        Version = "v1"
    });

    // ✅ JWT AUTH SUPPORT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
});

// ✅ DbContext
builder.Services.AddDbContext<HealthAxisDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<HealthAxisDbContext>()
    .AddDefaultTokenProviders();

// ✅ JWT AUTH
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

// ✅ Generic Repo
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// ✅ Patient
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

// ✅ Doctor
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

// ✅ Appointment
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// ✅ HealthRecord
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();

// ✅ Auth
builder.Services.AddScoped<IAuthService, AuthService>();

// ✅ AutoMapper
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// ✅ Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ Exception Middleware

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
app.UseExceptionHandler();


// ✅ Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ✅ Role Seeder
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRoles(roleManager);
}

app.Run();