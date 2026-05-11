using Microsoft.EntityFrameworkCore;
using TicketSystemAPI.Domain.Entities;

namespace TicketSystemAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<UserRole> UserRoles => Set<UserRole>();

        public DbSet<Ticket> Tickets => Set<Ticket>();

        public DbSet<TicketComment> TicketComments => Set<TicketComment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("core");

            modelBuilder.Entity<User>().ToTable("users");

            modelBuilder.Entity<Role>().ToTable("roles");

            modelBuilder.Entity<UserRole>().ToTable("user_roles");

            modelBuilder.Entity<Ticket>().ToTable("tickets");

            modelBuilder.Entity<TicketComment>().ToTable("ticket_comments");

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new
                {
                    ur.UserId,
                    ur.RoleId
                });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}