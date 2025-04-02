using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<WatchlistItem> Watchlist { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}