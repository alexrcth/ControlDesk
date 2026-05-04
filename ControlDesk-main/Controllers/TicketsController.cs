using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSystemAPI.Application.Interfaces;
using TicketSystemAPI.Domain.Entities;

namespace TicketSystemAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _repository;

        public TicketsController(ITicketRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _repository.GetAllAsync();
            return Ok(tickets);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Ticket ticket)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.AddAsync(ticket);
            return CreatedAtAction(nameof(GetAll), ticket);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Support")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var updated = await _repository.UpdateStatusAsync(id, status);
            if (!updated)
                return NotFound(new { message = $"Ticket {id} no encontrado." });

            return Ok(new { message = "Estado actualizado.", status });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Ticket {id} no encontrado." });

            return NoContent();
        }
    }
}