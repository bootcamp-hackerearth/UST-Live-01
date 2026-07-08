using HealthCare.Api.Consumers;
using HealthCare.Api.Data;
using HealthCare.Api.Mapping;
using HealthCare.Api.Middleware;
using HealthCare.Api.Repositories.Implementations;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
        private static async Task Main(string[] args)
        {
            //appconfig
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/healthcare-api-.txt", 
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            try
            {
                Log.Information("Starting HealthCare API");

                // Configure MassTransit
                builder.Services.AddMassTransit(x =>
                {
                    // Register the AppointmentBookedConsumer
                    x.AddConsumer<AppointmentBookedConsumer>();

                    // Configure RabbitMQ transport
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        // RabbitMQ connection configuration
                        cfg.Host(builder.Configuration["RabbitMq:Host"] ?? "localhost", h =>
                        {
                            h.Username(builder.Configuration["RabbitMq:Username"] ?? "guest");
                            h.Password(builder.Configuration["RabbitMq:Password"] ?? "guest");
                        });

                        // Configure JSON serializer for message serialization
                        cfg.ConfigureJsonSerializerOptions(settings =>
                        {
                            settings.PropertyNameCaseInsensitive = true;
                            return settings;
                        });

                        // Configure receive endpoint for appointment booked events
                        cfg.ReceiveEndpoint("appointment-booked-queue", e =>
                        {
                            e.ConfigureConsumer<AppointmentBookedConsumer>(context);
                            e.PrefetchCount = 16;
                        });
                    });
                });

                //Mapping
                builder.Services.AddAutoMapper(cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                });

                //Hosted Servive
               // builder.Services.AddHostedService<HeartbeatService>();
                builder.Services.AddHostedService<NotificationCleanupService>();
                //Exception Handler
                builder.Services.AddProblemDetails();
                builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();

                //DB conn
                builder.Services.AddDbContext<HealthCareDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("Dbconn"))
                );

                //CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
                });


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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            // Set a breakpoint here in Visual Studio
                            Console.WriteLine($"JWT Error: {context.Exception.Message}");
                            return Task.CompletedTask;
                        }
                    };
                });

                builder.Services.AddAuthorization();
                builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                builder.Services.AddScoped<IPatientRepository, PatientRepository>();
                builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
                builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
                builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

                builder.Services.AddSwaggerGen();
                builder.Services.AddScoped<IAuthService, AuthService>();
                builder.Services.AddScoped<IPatientService, PatientService>();
                builder.Services.AddScoped<IDoctorService, DoctorService>();
                builder.Services.AddScoped<IAppointmentService, AppointmentService>();
                builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
                builder.Services.AddEndpointsApiExplorer();


                //swagger

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
                app.UseExceptionHandler();

                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    await RoleSeeder.SeedRoleAsync(roleManager);
                    await AdminSeeder.SeedAdminAsync( userManager,roleManager,builder.Configuration);

                }

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();

                }

                app.UseCors("AllowAll");

                app.UseHttpsRedirection();
                app.UseRouting();

                app.UseAuthentication();

                app.UseAuthorization();

                app.MapControllers();

                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}