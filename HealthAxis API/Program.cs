using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register HealthAxis DbContext.
builder.Services.AddDbContext<HealthAxisDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HealthAxisDb")));

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