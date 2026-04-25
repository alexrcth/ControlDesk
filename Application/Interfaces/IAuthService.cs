using TicketSystemAPI.Application.DTOs;
using TicketSystemAPI.Domain.Entities;

namespace TicketSystemAPI.Application.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        Task<User> RegisterAsync(RegisterDto dto);
        Task<User?> ValidateCredentialsAsync(LoginDto dto);
    }
}