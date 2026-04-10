using Microsoft.EntityFrameworkCore;
using TicketSystemAPI.Models;


namespace TicketSystemAPI.Data
{
public class AppDbContext : DbContext
{
public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


public DbSet<User> Users => Set<User>();
public DbSet<Ticket> Tickets => Set<Ticket>();
public DbSet<TicketComment> TicketComments => Set<TicketComment>();
}
}