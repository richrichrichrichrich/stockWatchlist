using System;
using System.ComponentModel.DataAnnotations;

public class WatchlistItem
{
    [Key]
    public int Id { get; set; }
    public string Ticker { get; set; }
    public decimal BuyPrice { get; set; }
    public decimal SellPrice { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string UserId { get; set; }
}