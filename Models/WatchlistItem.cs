using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsxWatchlist.Models
{
    public class WatchlistItem
    {
        public int Id { get; set; }

        [Required]
        public string Ticker { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TargetBuyPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TargetSellPrice { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime ExpiryDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? LastKnownPrice { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? LastUpdated { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public string UserId { get; set; } = string.Empty;

        // === Portfolio ===
        public int QuantityHeld { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AverageBuyPrice { get; set; }

        public string? UnfulfilledOrders { get; set; } // Could be JSON or CSV for now

        public string? LastTradeType { get; set; } // e.g., "Buy" or "Sell"
        public int LastTradeQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LastTradePrice { get; set; }

        public DateTime? LastTradeDate { get; set; }

        [MaxLength(500)]
        public string? LastTradeNote { get; set; }

        // === Analysis ===
        public string? Sector { get; set; }
        public string? Industry { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PERatio { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Dividend { get; set; }

        public string? InsiderBuying { get; set; }
        public string? InsiderSelling { get; set; }

        public string? Country { get; set; }
        public string? CapSize { get; set; }
        public string? Valuation { get; set; }

        public string? TrendGraphUrl { get; set; }

        [MaxLength(1000)]
        public string? AnalysisNotes { get; set; }
    }
}
