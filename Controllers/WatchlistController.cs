using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsxWatchlist.Data;
using AsxWatchlist.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AsxWatchlist.Services;

namespace AsxWatchlist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchlistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly StockPriceService _priceService;

        public WatchlistController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, StockPriceService priceService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _priceService = priceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchlistItem>>> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            return await _context.WatchlistItems
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WatchlistItem>> GetById(int id)
        {
            var item = await _context.WatchlistItems.FindAsync(id);
            if (item == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (item.UserId != userId) return Forbid();

            return item;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WatchlistItem item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            item.UserId = userId;
            item.Notes ??= "";

            _context.WatchlistItems.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] WatchlistItem item)
        {
            Console.WriteLine($"🛠️ PUT called: RouteId={id}, ItemId={item.Id}, Ticker={item.Ticker}, Buy={item.TargetBuyPrice}, Sell={item.TargetSellPrice}, UserId={item.UserId}");

            if (id != item.Id)
            {
                Console.WriteLine("❌ ID mismatch.");
                return BadRequest("ID mismatch.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                Console.WriteLine("❌ No authenticated user.");
                return Unauthorized();
            }

            if (item.UserId != userId)
            {
                Console.WriteLine($"❌ UserId mismatch. Expected {userId}, got {item.UserId}");
                return Forbid();
            }

            try
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                Console.WriteLine("✅ Item saved successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 Save failed: {ex.Message}");
                return BadRequest($"Save failed: {ex.Message}");
            }
        }


        // [HttpPut("{id}")]
        // public async Task<IActionResult> Put(int id, [FromBody] WatchlistItem item)
        // {
        //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //     if (item.UserId != userId)
        //     {
        //         Console.WriteLine($"❌ Forbidden: item.UserId = {item.UserId}, current user = {userId}");
        //         return Forbid();
        //     }


        //     if (id != item.Id) return BadRequest();

        //     if (item.UserId != userId) return Forbid();

        //     _context.Entry(item).State = EntityState.Modified;
        //     await _context.SaveChangesAsync();
        //     return NoContent();
        // }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.WatchlistItems.FindAsync(id);
            if (item == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (item.UserId != userId) return Forbid();

            _context.WatchlistItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id}/refresh")]
        public async Task<IActionResult> RefreshPrice(int id)
        {
            var item = await _context.WatchlistItems.FindAsync(id);
            if (item == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (item.UserId != userId) return Forbid();

            var price = await _priceService.GetLatestPriceAsync(item.Ticker);
            if (price == null)
            {
                return BadRequest("Could not fetch price or empty response.");
            }

            item.LastKnownPrice = price;
            item.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(item);
        }


    }
}
