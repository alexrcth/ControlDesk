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
    }
}