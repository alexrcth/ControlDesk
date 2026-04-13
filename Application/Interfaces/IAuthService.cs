using TicketSystemAPI.Domain.Entities;

namespace TicketSystemAPI.Application.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(User user);
    }
}