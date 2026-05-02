namespace TicketSystemAPI.Application.DTOs
{
    public class RegisterDto
    {
        public string Name     { get; set; } = string.Empty;
        public string Email    { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        // Role eliminado del DTO por seguridad
        // El rol siempre sera "User" al registrarse
    }
}