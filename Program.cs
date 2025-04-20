using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsxWatchlist.Models;
using AsxWatchlist.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Components;
using AsxWatchlist.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load config and secrets
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

// ✅ Show config diagnostics
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine("Loaded DB connection: " + connectionString);
Console.WriteLine("SMTP Host: " + builder.Configuration["EmailSettings:Host"]);

// ✅ Register services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddTransient<IEmailSender, EmailSender>();

// ✅ Fix: Prevent API from redirecting to login HTML page
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});
builder.Services.AddHttpClient("Default", (sp, client) =>
{
    var accessor = sp.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext?.Request;

    if (request != null)
    {
        var uri = new Uri($"{request.Scheme}://{request.Host}");
        client.BaseAddress = uri;

        // Copy auth cookie
        if (request.Headers.TryGetValue("Cookie", out var cookie))
        {
            client.DefaultRequestHeaders.Add("Cookie", cookie.ToString());
        }
    }
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    UseCookies = false // ⛔ disable automatic cookie container (we're doing it manually)
});

builder.Services.AddHttpClient<StockPriceService>();
builder.Services.AddScoped<StockPriceService>();


var app = builder.Build();

// ✅ Runtime middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ✅ Endpoints
app.MapControllers();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
