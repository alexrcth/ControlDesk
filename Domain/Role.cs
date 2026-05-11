using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystemAPI.Domain.Entities
{
    [Table("roles", Schema = "core")]
    public class Role
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; }
            = new List<UserRole>();
    }
}