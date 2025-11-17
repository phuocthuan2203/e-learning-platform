using Microsoft.EntityFrameworkCore;
using ElearningPlatform.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using ElearningPlatform.Core;
using ElearningPlatform.Core.Interfaces;
using ElearningPlatform.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Register Services to the DI Container ---

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy to allow requests from the Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular dev server URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

// Configure and register the DbContext for Entity Framework Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
        mySqlOptions => mySqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore))
);

// Bind JwtSettings from appsettings.json to the JwtSettings class
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddControllers();

var app = builder.Build();

// --- 2. Configure the HTTP Request Pipeline ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable the CORS policy
app.UseCors("AllowAngularApp");

app.MapControllers();

app.Run();
