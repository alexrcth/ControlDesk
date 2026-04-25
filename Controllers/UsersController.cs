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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
                .Select(u => new { u.Id, u.Name, u.Email, u.Role })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var name   = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var email  = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var role   = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new { userId, name, email, role });
        }

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