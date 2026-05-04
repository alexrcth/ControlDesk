using TicketSystemAPI.Domain.Entities;

namespace TicketSystemAPI.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetAllAsync();
        Task AddAsync(Ticket ticket);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStatusAsync(int id, string status);
    }
}