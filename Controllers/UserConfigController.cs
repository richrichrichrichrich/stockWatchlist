using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsxWatchlist.Data;
using AsxWatchlist.Models;
using System.Security.Claims;

namespace AsxWatchlist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserConfigController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserConfigController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserConfig>> Get(string userId)
        {
            var config = await _context.UserConfigs.FirstOrDefaultAsync(c => c.UserId == userId);

            if (config == null)
            {
                config = new UserConfig { UserId = userId };
                _context.UserConfigs.Add(config);
                await _context.SaveChangesAsync();
            }

            return config;
        }

        [HttpPut]
        public async Task<IActionResult> Put(UserConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.UserId))
                return BadRequest("UserId is required");

            var existing = await _context.UserConfigs.FirstOrDefaultAsync(c => c.UserId == config.UserId);

            if (existing == null)
            {
                _context.UserConfigs.Add(config);
            }
            else
            {
                _context.Entry(existing).CurrentValues.SetValues(config);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
