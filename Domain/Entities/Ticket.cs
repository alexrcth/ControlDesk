namespace TicketSystemAPI.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Open"; 
        public string? Comments { get; set; }
        
        public bool IsActive { get; set; } = true; 
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}