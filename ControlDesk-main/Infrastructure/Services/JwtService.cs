using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketSystemAPI.Application.DTOs;
using TicketSystemAPI.Application.Interfaces;
using TicketSystemAPI.Domain.Entities;
using TicketSystemAPI.Infrastructure.Data;

namespace TicketSystemAPI.Infrastructure.Services
{
    public class JwtService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public JwtService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string GenerateToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key no esta configurado.");
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email.ToLower());
            if (exists) throw new InvalidOperationException("El email ya esta registrado.");
            var user = new User
            {
                Name = dto.Name.Trim(),
                Email = dto.Email.ToLower().Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> ValidateCredentialsAsync(LoginDto dto)
        {
            User? user = null;
            if (!string.IsNullOrEmpty(dto.Email))
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower().Trim());
            if (user == null && !string.IsNullOrEmpty(dto.Username))
                user = await _context.Users.FirstOrDefaultAsync(u => u.Name == dto.Username.Trim());
            if (user == null) return null;
            var passwordValida = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            return passwordValida ? user : null;
        }
    }
}
