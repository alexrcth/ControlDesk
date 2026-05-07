namespace TicketSystemAPI.Domain.Entities
{
public class Ticket
{
public int Id { get; set; }
public string Title { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string Status { get; set; } = "Open"; 
public string Urgency { get; set; } = "Baja";
public int UserId { get; set; }
public User? User { get; set; }
public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
}
