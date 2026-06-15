using HealthAxisCore_Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//  Add services to the container
builder.Services.AddControllers();

//  Register DbContext with SQL Server
builder.Services.AddDbContext<HealthAppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Authorization middleware
app.UseAuthorization();

//  Map controllers
app.MapControllers();

app.Run();