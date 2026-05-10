using Microsoft.EntityFrameworkCore;
using TicketSystemAPI.Domain.Entities;

namespace TicketSystemAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<TicketComment> TicketComments => Set<TicketComment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Schema PostgreSQL
            modelBuilder.HasDefaultSchema("core");

            // Tablas
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Ticket>().ToTable("tickets");
            modelBuilder.Entity<TicketComment>().ToTable("ticket_comments");

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}