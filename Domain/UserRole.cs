using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystemAPI.Domain.Entities
{
    [Table("user_roles", Schema = "core")]
    public class UserRole
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("role_id")]
        public Guid RoleId { get; set; }

        public User User { get; set; } = null!;

        public Role Role { get; set; } = null!;
    }
}