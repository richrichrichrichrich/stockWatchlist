using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine("💌 Sending email to: " + email);
        Console.WriteLine("SMTP Host: " + _configuration["EmailSettings:Host"]);
        Console.WriteLine("SMTP User: " + _configuration["EmailSettings:UserName"]);


        var smtpClient = new SmtpClient
        {
            Host = _configuration["EmailSettings:Host"], // 💥 This is null if not set!
            Port = int.Parse(_configuration["EmailSettings:Port"] ?? "587"),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _configuration["EmailSettings:UserName"],
                _configuration["EmailSettings:Password"])
        };

        return smtpClient.SendMailAsync(new MailMessage(
        //_configuration["EmailSettings:UserName"], // use sender from config
        "richrichrichrichrich@gmail.com",
        email,
        subject,
        htmlMessage
    )
        {
            IsBodyHtml = true
        });
    }
}