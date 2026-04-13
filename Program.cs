using Microsoft.EntityFrameworkCore;
using TicketSystemAPI.Infrastructure.Data;
using TicketSystemAPI.Application.Interfaces;
using TicketSystemAPI.Infrastructure.Repositories;
using TicketSystemAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Base de datos (temporal en memoria)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TicketDb"));

// Repositorios
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// JWT Service
builder.Services.AddScoped<IAuthService, JwtService>();

// 🔐 CONFIG JWT
var key = Encoding.ASCII.GetBytes("super_secret_key_12345");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication(); // 👈 importante
app.UseAuthorization();

app.MapControllers();

app.Run();