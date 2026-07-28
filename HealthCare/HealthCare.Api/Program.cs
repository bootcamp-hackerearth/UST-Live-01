using HealthCare.Api.Consumers;
using HealthCare.Api.Data;
using HealthCare.Api.Mapping;
using HealthCare.Api.Middleware;
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
        private static async Task Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            try
            {
                Log.Information("Starting HealthCare API");

                //appconfig
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddSerilog((services, configuration) =>
                    configuration.ReadFrom.Configuration(builder.Configuration)
                                 .ReadFrom.Services(services)
                                 .Enrich.FromLogContext());
                                 
                // Configure MassTransit reg using IBus
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
                var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAllClients", policy =>
                    {
                        policy.WithOrigins(allowedOrigins ?? Array.Empty<string>()).AllowAnyHeader().AllowAnyMethod();
                    });
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

                //APP BUILD
                var app = builder.Build();

                app.UseSerilogRequestLogging();
                app.UseExceptionHandler();

                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    await RoleSeeder.SeedRoleAsync(roleManager);
                    await AdminSeeder.SeedAdminAsync(userManager, roleManager, builder.Configuration);

                }

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();

                }

                app.UseCors("AllowAllClients");

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthentication();

                app.UseAuthorization();

                //deployment
                var contentTypeProvider=new FileExtensionContentTypeProvider();
                contentTypeProvider.Mappings[".data"] = "application/octet-stream";
                contentTypeProvider.Mappings[".wasm"] = "application/wasm";
                app.UseStaticFiles(new StaticFileOptions{ ContentTypeProvider = contentTypeProvider });

                app.MapControllers();

                app.MapGet("/", context =>
                {
                    context.Response.Redirect("/angular");

                    return Task.CompletedTask;
                });
                app.MapGet("/angular", async context =>
                {
                    await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "angular","index.html"));
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

                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
    }
}