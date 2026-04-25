namespace TicketSystemAPI.Domain.Entities
{
public class TicketComment
{
public int Id { get; set; }
public string Message { get; set; } = string.Empty;
public int TicketId { get; set; }
public Ticket? Ticket { get; set; }
public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
}