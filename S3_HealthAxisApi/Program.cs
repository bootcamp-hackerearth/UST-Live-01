using HealthAxis.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using S3_HealthAxisApi.Repository.Implementation;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using S3_HealthAxisApi.Services.Interface;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

#region Controllers

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            JsonNamingPolicy.CamelCase;
    });

#endregion

#region Swagger

builder.Services.AddEndpointsApiExplorer();
#region Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1",
//        new OpenApiInfo
//        {
//            Title = "HealthAxis API",
//            Version = "v1"
//        });

//    options.AddSecurityDefinition(
//        "Bearer",
//        new OpenApiSecurityScheme
//        {
//            Name = "Authorization",
//            Type = SecuritySchemeType.Http,
//            Scheme = "bearer",
//            BearerFormat = "JWT",
//            In = ParameterLocation.Header,
//            Description =
//                "Enter JWT Token. Example: Bearer eyJhbGc..."
//        });

//    options.AddSecurityRequirement(
//        new OpenApiSecurityRequirement
//        {
//            {
//                new OpenApiSecurityScheme
//                {
//                    Reference = new OpenApiReference
//                    {
//                        Type = ReferenceType.SecurityScheme,
//                        Id = "Bearer"
//                    }
//                },
//                Array.Empty<string>()
//            }
//        });
//});

#endregion

#region Database

builder.Services.AddDbContext<HealthAxisDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

#endregion

#region JWT Authentication

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwt["Issuer"],

                ValidateAudience = true,
                ValidAudience = jwt["Audience"],

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt["Key"]!)),

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

#endregion

#region Repositories

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion

#region Services

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
builder.Services.AddScoped<IAuthService, AuthService>();

#endregion

var app = builder.Build();

#region Middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "HealthAxis API v1");

        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();