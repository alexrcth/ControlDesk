using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystemAPI.Data;
using TicketSystemAPI.Models;


namespace TicketSystemAPI.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
private readonly AppDbContext _context;


public TicketsController(AppDbContext context)
{
_context = context;
}


[HttpGet]
public async Task<IActionResult> GetAll()
{
var tickets = await _context.Tickets.Include(t => t.User).ToListAsync();
return Ok(tickets);
}


[HttpPost]
public async Task<IActionResult> Create(Ticket ticket)
{
_context.Tickets.Add(ticket);
await _context.SaveChangesAsync();
return Ok(ticket);
}
}
}