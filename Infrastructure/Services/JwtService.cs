using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
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
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            await using var connection = new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            var checkCmd = new NpgsqlCommand(
                "SELECT COUNT(*) FROM core.users WHERE email = @email",
                connection);

            checkCmd.Parameters.AddWithValue("email", dto.Email.ToLower().Trim());

            var exists = (long)(await checkCmd.ExecuteScalarAsync() ?? 0);

            if (exists > 0)
                throw new InvalidOperationException("El email ya esta registrado.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.Name.Trim(),
                LastName = "Default",
                Email = dto.Email.ToLower().Trim(),
                PhoneNumber = "00000000",
                DateOfBirth = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var insertCmd = new NpgsqlCommand(@"
                INSERT INTO core.users
                (
                    id,
                    first_name,
                    last_name,
                    email,
                    phone_number,
                    date_of_birth,
                    created_at,
                    updated_at,
                    is_active
                )
                VALUES
                (
                    @id,
                    @first_name,
                    @last_name,
                    @email,
                    @phone_number,
                    @date_of_birth,
                    @created_at,
                    @updated_at,
                    @is_active
                )",
                connection);

            insertCmd.Parameters.AddWithValue("id", user.Id);
            insertCmd.Parameters.AddWithValue("first_name", user.FirstName);
            insertCmd.Parameters.AddWithValue("last_name", user.LastName);
            insertCmd.Parameters.AddWithValue("email", user.Email);
            insertCmd.Parameters.AddWithValue("phone_number", user.PhoneNumber);
            insertCmd.Parameters.AddWithValue("date_of_birth", user.DateOfBirth);
            insertCmd.Parameters.AddWithValue("created_at", user.CreatedAt);
            insertCmd.Parameters.AddWithValue("updated_at", user.UpdatedAt);
            insertCmd.Parameters.AddWithValue("is_active", user.IsActive);

            await insertCmd.ExecuteNonQueryAsync();

            Console.WriteLine("Usuario registrado correctamente.");

            return user;
        }

        public async Task<User?> ValidateCredentialsAsync(LoginDto dto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            await using var connection = new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            var cmd = new NpgsqlCommand(@"
                SELECT
                    id,
                    first_name,
                    last_name,
                    email,
                    phone_number,
                    date_of_birth,
                    created_at,
                    updated_at,
                    is_active
                FROM core.users
                WHERE email = @email
                LIMIT 1",
                connection);

            cmd.Parameters.AddWithValue("email", dto.Email.ToLower().Trim());

            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            var user = new User
            {
                Id = reader.GetGuid(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                PhoneNumber = reader.GetString(4),
                DateOfBirth = reader.GetDateTime(5),
                CreatedAt = reader.GetDateTime(6),
                UpdatedAt = reader.GetDateTime(7),
                IsActive = reader.GetBoolean(8)
            };

            return user;
        }
    }
}