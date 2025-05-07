using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AsxWatchlist.Models
{
    public class UserConfig
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DefaultTradeAmount { get; set; } = 400;

        [Range(0, double.MaxValue)]
        public decimal MaxHoldingAmount { get; set; } = 1000;

        [Required]
        public string CheckTradeFrequency { get; set; } = "30 mins";

        [Required]
        public string CheckHoldingsFrequency { get; set; } = "30 mins";

        [Range(1, 365)]
        public int DefaultExpiryLengthDays { get; set; } = 30;

        public TimeSpan TradingDayStart { get; set; } = new TimeSpan(12, 0, 0);
        public TimeSpan TradingDayEnd { get; set; } = new TimeSpan(19, 0, 0);


        [NotMapped]
        public string TradingDayStartString
        {
            get => TradingDayStart.ToString(@"hh\:mm");
            set => TradingDayStart = TimeSpan.TryParse(value, out var ts) ? ts : new TimeSpan(12, 0, 0);
        }

        [NotMapped]
        public string TradingDayEndString
        {
            get => TradingDayEnd.ToString(@"hh\:mm");
            set => TradingDayEnd = TimeSpan.TryParse(value, out var ts) ? ts : new TimeSpan(19, 0, 0);
        }


        public bool CheckMonday { get; set; } = true;
        public bool CheckTuesday { get; set; } = true;
        public bool CheckWednesday { get; set; } = true;
        public bool CheckThursday { get; set; } = true;
        public bool CheckFriday { get; set; } = true;
        public bool CheckSaturday { get; set; } = false;
        public bool CheckSunday { get; set; } = false;

        public bool ShowOneExpandedTicker { get; set; } = true;
        [Range(0, 100)]
        public decimal StopLossPercent { get; set; } = 5; // default to 5%


    }
}
