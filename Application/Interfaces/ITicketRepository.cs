using TicketSystemAPI.Domain.Entities;

namespace TicketSystemAPI.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetAllAsync();
        Task AddAsync(Ticket ticket);
    }
}