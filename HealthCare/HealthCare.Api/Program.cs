using HealthCare.Api.Data;
using HealthCare.Api.Mapping;
using HealthCare.Api.Repositories.Implementations;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

//appconfig
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

//DB conn
builder.Services.AddDbContext<HealthCareDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Dbconn"))
);


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<HealthCareDbContext>()
    .AddDefaultTokenProviders();

// JWT Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
        ),
        RoleClaimType = ClaimTypes.Role,
        ClockSkew = TimeSpan.Zero
    };
});



builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository,DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository,AppointmentRepository>();
builder.Services.AddScoped<IHealthRecordRepository,HealthRecordRepository>();

builder.Services.AddScoped<IPatientService,PatientService>();
builder.Services.AddScoped<IDoctorService,DoctorService>();
builder.Services.AddScoped<IAppointmentService,AppointmentService>();
builder.Services.AddScoped<IHealthRecordService,HealthRecordService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddOpenApi();

var app = builder.Build();

//

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
