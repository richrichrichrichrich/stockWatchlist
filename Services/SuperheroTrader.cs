using Microsoft.Extensions.Options;
using AsxWatchlist.Models;
using Microsoft.Playwright;
using System.ComponentModel.DataAnnotations;



namespace AsxWatchlist.Services
{
    public class SuperheroTrader
    {
        private readonly SuperheroConfig _config;
        private const string ProfileDir = "superhero-profile";
        private IPlaywright? _playwright;
        private IBrowserContext? _context;
        private IPage? _superheroPage;
        private IBrowser? _browser;


        public SuperheroTrader(IOptions<SuperheroConfig> options)
        {
            _config = options.Value;
        }

        private async Task EnsureBrowserReadyAsync()
        {
            _playwright ??= await Playwright.CreateAsync();

            if (_context == null)
            {
                _context = await _playwright.Chromium.LaunchPersistentContextAsync(ProfileDir, new()
                {
                    Headless = false,
                    SlowMo = 50,
                    Args = new[] { "--window-position=3000,0", "--window-size=200,200" }
                });

                _browser = _context.Browser;
            }

            _superheroPage = _context.Pages.FirstOrDefault() ?? await _context.NewPageAsync();
        }

        private async Task<IPage> EnsureLoggedInAsync()
        {

            await EnsureBrowserReadyAsync();

            if (_superheroPage == null)
                throw new InvalidOperationException("Superhero page is not initialized.");

            try
            {
                await _superheroPage.GotoAsync("https://app.superhero.com.au/dashboard", new()
                {
                    WaitUntil = WaitUntilState.Load,
                    Timeout = 10000
                });
            }
            catch (TimeoutException)
            {
                Console.WriteLine("⚠️ Dashboard page didn't fully load — assuming login is required.");
            }

            if (_superheroPage.Url.Contains("/dashboard"))
            {
                // Check if login form is still visible on dashboard (unauthenticated)
                var loginField = _superheroPage.Locator("input[name='email']");
                if (await loginField.IsVisibleAsync(new() { }))
                {
                    Console.WriteLine("🔐 Still on login page (email field visible), continuing login process...");
                }
                else
                {
                    Console.WriteLine("✅ Already logged in!");
                    return _superheroPage;
                }
            }


            Console.WriteLine("🔐 Logging in...");

            await _superheroPage.GotoAsync("https://app.superhero.com.au/log-in");
            await _superheroPage.FillAsync("input[name='email']", _config.Email);
            await _superheroPage.FillAsync("input[name='password']", _config.Password);
            await _superheroPage.ClickAsync("button[type='submit']");

            await _superheroPage.WaitForURLAsync("**/dashboard", new() { Timeout = 30000 });

            await _context!.StorageStateAsync(new() { Path = "superhero-session.json" });

            return _superheroPage;
        }

        public async Task PlaceBuyOrder(string ticker, decimal totalCost, decimal price, bool marketOrder)
        {
            _superheroPage = await EnsureLoggedInAsync();

            try
            {
                Console.WriteLine($"📦 Placing buy order for {ticker}");
                await _superheroPage.BringToFrontAsync();

                var acceptBtn = _superheroPage.Locator("button span:text('Accept')");
                if (await acceptBtn.IsVisibleAsync())
                {
                    await acceptBtn.ClickAsync(new() { Timeout = 3000, Force = true });
                }

                var searchBox = _superheroPage.Locator("input[placeholder='Search Shares, Themes and ETFs']").Nth(1);
                await searchBox.ClickAsync(new() { Force = true });
                await searchBox.FillAsync(ticker);
                Console.WriteLine("🔍 Searching for ticker...");

                var tickerResult = _superheroPage.Locator($"p:has-text('{ticker}.AU')");
                await tickerResult.WaitForAsync(new() { Timeout = 6000 });
                await tickerResult.ClickAsync(new() { Force = true });
                Console.WriteLine("🔍 Selected ticker..." + ticker);


                var buyBtn = _superheroPage.GetByRole(AriaRole.Button, new() { Name = "Buy" });
                await buyBtn.ClickAsync(new() { Timeout = 5000 });
                Console.WriteLine("🔍 Clicked on Buy button...");


                var orderTypeDiv = _superheroPage.Locator("div.fcgZKd", new() { HasText = "Order Type" });
                await orderTypeDiv.ClickAsync(new() { Timeout = 3000, Force = true });
                Console.WriteLine("🔍 Clicked order type field...");


                if (marketOrder)
                {
                    var marketOrderOptions = _superheroPage.Locator("div.FilterWithDropDown__OptionButton-sc-bnii75-0:has(h3:text-is('Market Order'))");
                    await ClickFirstVisibleAsync(marketOrderOptions);
                    Console.WriteLine("🛒 Market Order selected");
                }
                else
                {
                    var limitOption = _superheroPage.Locator("div.FilterWithDropDown__OptionButton-sc-bnii75-0:has(h3:text-is('Limit Order'))");
                    await ClickFirstVisibleAsync(limitOption);
                    await _superheroPage.FillAsync("input[name='order_price']", price.ToString("F2"));
                    Console.WriteLine("💰 Limit price entered");
                }

                var orderTotalInput = _superheroPage.Locator("label:has-text('Amount to Invest')").Locator("xpath=..").Locator("input");
                await orderTotalInput.WaitForAsync(new() { Timeout = 6000 });
                await ClickFirstVisibleAsync(orderTotalInput);
                await orderTotalInput.FillAsync(totalCost.ToString("F2"));
                Console.WriteLine("💰 Order total entered");


                var reviewButton = _superheroPage.Locator("button").Filter(new() { HasText = "Review" });
                await ClickFirstVisibleAsync(reviewButton);

                await _superheroPage.WaitForTimeoutAsync(5000);

                var confirmButton = _superheroPage.GetByRole(AriaRole.Button, new() { Name = "Confirm" });
                await ClickFirstVisibleAsync(confirmButton);

                Console.WriteLine("✅ Order review complete");

                if (!string.IsNullOrEmpty(_config.PIN))
                {
                    string pin = _config.PIN;
                    foreach (char digit in pin)
                    {
                        await _superheroPage.GetByRole(AriaRole.Button, new() { Name = digit.ToString() }).ClickAsync();
                    }
                    Console.WriteLine("✅ PIN entered. Order confirmed.");


                }
                else
                {
                    Console.WriteLine("⚠️ No PIN provided, skipping PIN entry.");
                }

                var backToDash = _superheroPage.Locator("a[href='/dashboard']:has-text('Back to Dashboard')");
                await ClickFirstVisibleAsync(backToDash);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error placing order for {ticker}: {ex.Message}");
            }
        }

        public async Task CloseBrowserAsync()
        {
            try
            {
                if (_context != null)
                    await _context.CloseAsync();

                if (_browser != null)
                    await _browser.CloseAsync();

                _playwright?.Dispose();
            }
            catch
            {
                // Optional: log cleanup error
            }

            _context = null;
            _browser = null;
            _playwright = null;
            _superheroPage = null;

            Console.WriteLine("🧹 Browser context closed");
        }

        private async Task<bool> ClickFirstVisibleAsync(ILocator locator)
        {
            int count = await locator.CountAsync();
            for (int i = 0; i < count; i++)
            {
                var option = locator.Nth(i);
                if (await option.IsVisibleAsync())
                {
                    await option.ClickAsync();
                    return true;
                }
            }
            return false;
        }
    }



}
