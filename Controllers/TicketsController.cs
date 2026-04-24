using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSystemAPI.Application.Interfaces;
using TicketSystemAPI.Domain.Entities;

namespace TicketSystemAPI.Controllers
{   [Authorize]
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
        public async Task<IActionResult> Create(Ticket ticket)
        {
            await _repository.AddAsync(ticket);
            return Ok(ticket);
        }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Ticket ticket)
    {
        if (id != ticket.Id) return BadRequest("El ID no coincide");

        if (string.IsNullOrWhiteSpace(ticket.Title) || string.IsNullOrWhiteSpace(ticket.Description))
        {
            return BadRequest("El título y la descripción son obligatorios.");
        }

        await _repository.UpdateAsync(ticket);
        return Ok(ticket);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Disable(int id)
    {
        var ticket = await _repository.GetByIdAsync(id);
        if (ticket == null) return NotFound("Ticket no encontrado");

        ticket.IsActive = false; 
        ticket.Status = "Closed"; 
        
        await _repository.UpdateAsync(ticket);
        return Ok(new { message = "Ticket deshabilitado correctamente", ticket });
    }
    }
}