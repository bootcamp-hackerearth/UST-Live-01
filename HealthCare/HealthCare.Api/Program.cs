using HealthCare.Api.Data;
using HealthCare.Api.Mapping;
using HealthCare.Api.Messaging;
using HealthCare.Api.Middleware;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Implementations;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Security.Claims;
using System.Text;

namespace HealthCare.Api
{
    public partial class Program
    {
        public static async Task Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
.WriteTo.Console().CreateBootstrapLogger();

            Log.Information(" HealthCareApp Api Starting......");

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, services, configuration) =>
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services));


            // Add services to the container.
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            builder.Services.AddDbContext<HealthCareDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<HealthCareDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
            {
                var jwt = builder.Configuration.GetSection("Jwt");

                option.TokenValidationParameters = new TokenValidationParameters
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
                    NameClaimType = ClaimTypes.NameIdentifier,

                    ClockSkew = TimeSpan.Zero
                };
            });

            // Repositories
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

            // Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
            builder.Services.AddScoped<IJwtService, JwtService>();


            var rabbitmqConfig = builder.Configuration.GetSection("RabbitMq");
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<AppointmentBookedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {

                    cfg.Host(rabbitmqConfig["HostName"],
                        rabbitmqConfig["VirtualHost"], h =>
                    {
                        h.Username(rabbitmqConfig["UserName"]!);
                        h.Password(rabbitmqConfig["Password"]!);
                    });

                    cfg.ReceiveEndpoint(rabbitmqConfig["AppointmentQueue"]!, e =>
                    {
                        // Retry the message 3 times with a 5-second interval
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(3, TimeSpan.FromSeconds(5));
                        });

                        e.ConfigureConsumer<AppointmentBookedConsumer>(context);
                    });
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowClients", policy =>
                {
                    policy
                        .WithOrigins(
                            "https://localhost:7058",
                            "http://localhost:59971"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
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
                    Description = "Enter JWT token only. Do not type Bearer."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });
            });

            var app = builder.Build();

            app.UseSerilogRequestLogging();
            app.UseExceptionHandler();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<User>>();

                await RoleSeeder.SeedRolesAsync(roleManager);
                await AdminSeeder.SeedAdminAsync(userManager, roleManager);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            //deployment
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings[".dat"] = "application/octet-stream";
            contentTypeProvider.Mappings[".wasm"] = "application/wasm";
            app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = contentTypeProvider });

            app.UseRouting();

            app.UseCors("AllowClients");

            app.UseAuthentication();


            app.UseAuthorization();

            app.MapControllers();
            app.MapGet("/angular", async context =>
            {
                await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "angular", "index.html"));
            });
            app.MapGet("/angular/{*path:nonfile}", async context =>
            {
                await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "angular", "index.html"));
            });
            app.MapGet("/blazor", async context =>
            {
                await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "blazor", "index.html"));
            });
            app.MapGet("/blazor/{*path:nonfile}", async context =>
            {
                await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "blazor", "index.html"));
            });


            try
            {
                await app.RunAsync();
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
    }
}