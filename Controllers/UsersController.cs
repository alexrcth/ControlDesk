using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystemAPI.Infrastructure.Data;

namespace TicketSystemAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // Solo Admin puede ver todos los usuarios
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
                .Select(u => new { u.Id, u.Name, u.Email, u.Role })
                .ToListAsync();

            return Ok(users);
        }

        // Cualquier usuario autenticado puede ver su perfil
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var name   = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var email  = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var role   = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new { userId, name, email, role });
        }

        // Solo Admin puede cambiar el rol de un usuario
        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] string newRole)
        {
            var rolesPermitidos = new[] { "Admin", "Support", "User" };
            if (!rolesPermitidos.Contains(newRole))
                return BadRequest(new { message = $"Rol invalido. Roles permitidos: {string.Join(", ", rolesPermitidos)}" });

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Usuario no encontrado." });

            user.Role = newRole;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Rol actualizado a '{newRole}'.", userId = user.Id, name = user.Name, role = user.Role });
        }

        // Solo Admin puede eliminar usuarios
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Usuario no encontrado." });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}