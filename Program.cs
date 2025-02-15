using Microsoft.EntityFrameworkCore;
using SocialMediaApp.BusinessLogic.Extensions;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json")
	.Build();

var connectionString = config.GetConnectionString("DatabaseConnection");
builder.Services.ConfigureDbContext(connectionString);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
