using Microsoft.AspNetCore.Identity.UI.Services;

namespace Kontravers.GoodJob.Auth;

public class EmailSender : IEmailSender
{
    private readonly Domain.IEmailSender _emailSender;

    public EmailSender(Kontravers.GoodJob.Domain.IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }
    
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return _emailSender.SendEmailAsync(email, subject, htmlMessage);
    }
}