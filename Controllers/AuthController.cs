using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystemAPI.Application.Interfaces;
using TicketSystemAPI.Infrastructure.Data;
using TicketSystemAPI.Domain.Entities;

namespace TicketSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == username);

            if (user == null)
                return Unauthorized("Usuario no encontrado");

            var token = _authService.GenerateToken(user);

            return Ok(new { token });
        }
    }
}