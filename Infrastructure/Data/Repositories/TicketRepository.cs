using Microsoft.EntityFrameworkCore;
using TicketSystemAPI.Application.Interfaces;
using TicketSystemAPI.Domain.Entities;
using TicketSystemAPI.Infrastructure.Data;
namespace TicketSystemAPI.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task AddAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }
    }
}