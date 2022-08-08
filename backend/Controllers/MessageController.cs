using FinancialChat.Data;
using FinancialChat.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MessageController : Controller
    {
        private readonly AuthDbContext _context;

        public MessageController(AuthDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var messages = await _context.Messages.ToListAsync();

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return Created("", null);
        }
    }
}