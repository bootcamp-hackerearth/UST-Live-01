using HealthCare.Api.Data;
using HealthCare.Api.Mapping;
using HealthCare.Api.Middleware;
using HealthCare.Api.Repositories.Implementations;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        //appconfig
        var builder = WebApplication.CreateBuilder(args);

        //Mapping
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        //swagger

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HealthApp API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT token only. Do not type Bearer."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });
        });


        //Exception Handler
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddControllers();

        //DB conn
        builder.Services.AddDbContext<HealthCareDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Dbconn"))
        );
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
        })
            .AddEntityFrameworkStores<HealthCareDbContext>().AddDefaultTokenProviders();

        // JWT Auth
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
                ),
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
        });

        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IPatientRepository, PatientRepository>();
        builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
        builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

        builder.Services.AddScoped<IPatientService, PatientService>();
        builder.Services.AddScoped<IDoctorService, DoctorService>();
        builder.Services.AddScoped<IAppointmentService, AppointmentService>();
        builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IAuthService, AuthService>();

        var app = builder.Build();
        app.UseExceptionHandler();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await RoleSeeder.SeedRoleAsync(roleManager);
            await AdminSeeder.SeedAdminAsync(userManager, roleManager);
        }

        if (app.Environment.IsDevelopment())
        {
            // app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();

        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}