using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMediaApp.BusinessLogic.Extensions; // Your extension methods
using SocialMediaApp.SignalR;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Configuration ---
var config = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Add reloadOnChange
	.AddEnvironmentVariables() // Good practice
	.Build();

// --- Database Context ---
var connectionString = config.GetConnectionString("DatabaseConnection");
builder.Services.ConfigureDbContext(connectionString); // Using your extension method

// --- Services ---
builder.Services.AddBusinessService();  // Using your extension method

//--- SignalR ---
builder.Services.AddSignalR();

// --- Controllers ---
builder.Services.AddControllers();

// --- OpenAPI/Swagger ---
builder.Services.AddEndpointsApiExplorer(); // For Swagger
builder.Services.AddSwaggerGen(c =>  // Enhanced Swagger setup
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1" });

	// Add JWT Authentication support to Swagger UI
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});

// --- JWT Authentication ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false, // For simplicity; set to true in production
			ValidateAudience = false, // For simplicity; set to true in production
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			//ValidIssuer = config["JWT:Issuer"], // Use in production
			//ValidAudience = config["JWT:Audience"], // Use in production
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecurityKey"]))
		};
	});

// CORS (Cross-Origin Resource Sharing - Important if your frontend is on a different domain/port)
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		builder =>
		{
			builder.WithOrigins("http://localhost:4200") // Replace with your frontend's URL
				   .AllowAnyHeader()
				   .AllowAnyMethod();
		});
});

var app = builder.Build();

// --- Configure the HTTP request pipeline ---
if (app.Environment.IsDevelopment())
{
	app.UseSwagger(); // Enable Swagger
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media API V1");
		// c.RoutePrefix = string.Empty; // Optional: Serve Swagger UI at the root
	});
}

app.UseHttpsRedirection();

app.UseRouting(); // Important: UseRouting must come before UseAuthentication and UseAuthorization

app.UseCors("AllowSpecificOrigin"); // Use CORS

app.UseAuthentication(); // MUST come before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.MapHub<MessageHub>("/hubs/message");
app.MapHub<FriendshipHub>("/hubs/friendships");

app.Run();