using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class StockPriceChecker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public StockPriceChecker(IServiceProvider serviceProvider, HttpClient httpClient, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                await CheckStockPrices(context, userManager);
            }
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private async Task CheckStockPrices(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        var watchlist = await context.Watchlist.ToListAsync();
        foreach (var item in watchlist)
        {
            var price = await GetStockPrice(item.Ticker);
            if (price != null && (price <= item.BuyPrice || price >= item.SellPrice))
            {
                var user = await userManager.FindByIdAsync(item.UserId);
                if (user != null)
                {
                    SendEmailNotification(user.Email, item.Ticker, price.Value);
                }
            }
        }
    }

    private async Task<decimal?> GetStockPrice(string ticker)
    {
        string apiKey = _configuration["AlphaVantage:ApiKey"];
        string url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={ticker}.AX&apikey={apiKey}";

        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(json);
            if (document.RootElement.TryGetProperty("Global Quote", out JsonElement quote) &&
                quote.TryGetProperty("05. price", out JsonElement priceElement))
            {
                return decimal.Parse(priceElement.GetString());
            }
        }
        return null;
    }

    private void SendEmailNotification(string email, string ticker, decimal price)
    {
        string smtpServer = _configuration["Email:SmtpServer"];
        int smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
        string smtpUser = _configuration["Email:SmtpUser"];
        string smtpPass = _configuration["Email:SmtpPass"];

        using (var client = new SmtpClient(smtpServer, smtpPort))
        {
            client.Credentials = new NetworkCredential(smtpUser, smtpPass);
            client.EnableSsl = true;

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(smtpUser),
                Subject = $"Stock Alert: {ticker}",
                Body = $"The stock {ticker} has reached a price of {price:C}"
            };
            mailMessage.To.Add(email);

            client.Send(mailMessage);
        }
    }
}