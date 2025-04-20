using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace AsxWatchlist.Services
{
    public class StockPriceService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<StockPriceService> _logger;

        public StockPriceService(HttpClient httpClient, IConfiguration config, ILogger<StockPriceService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = config["AlphaVantage:ApiKey"] ?? throw new InvalidOperationException("API key not configured");
        }

        public async Task<decimal?> GetLatestPriceAsync(string symbol)
        {
            var url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Alpha Vantage returned HTTP {StatusCode} for symbol {Symbol}", response.StatusCode, symbol);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning("Empty response from Alpha Vantage for symbol {Symbol}", symbol);
                return null;
            }

            try
            {
                var json = JsonDocument.Parse(content);
                if (json.RootElement.TryGetProperty("Global Quote", out var quote) &&
                    quote.TryGetProperty("05. price", out var priceEl) &&
                    decimal.TryParse(priceEl.GetString(), out var price))
                {
                    return price;
                }

                _logger.LogWarning("Could not find '05. price' for symbol {Symbol}", symbol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while parsing Alpha Vantage response");
            }

            return null;
        }
    }
}
