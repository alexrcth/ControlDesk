using Microsoft.AspNetCore.Mvc;
using TicketSystemAPI.Application.DTOs;
using TicketSystemAPI.Application.Interfaces;

namespace TicketSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _authService.RegisterAsync(dto);

                var token = _authService.GenerateToken(user);

                return CreatedAtAction(nameof(Register), new
                {
                    message = "Usuario registrado exitosamente.",
                    userId = user.Id,
                    name = user.FirstName,
                    role = "User",
                    token
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.ValidateCredentialsAsync(dto);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Credenciales inválidas."
                });
            }

            var token = _authService.GenerateToken(user);

            return Ok(new
            {
                message = "Login exitoso.",
                userId = user.Id,
                name = user.FirstName,
                role = "User",
                token
            });
        }
    }
}