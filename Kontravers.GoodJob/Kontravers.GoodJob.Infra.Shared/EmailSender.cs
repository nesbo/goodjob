using System.Net;
using System.Net.Mail;
using System.Text;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Work;

namespace Kontravers.GoodJob.Infra.Shared;

public class EmailSender : IEmailSender
{
    private const string From = "dev@kontrave.rs";
    private const string GjJobEmailTemplateHtml = "GJ-job-email-template_01.html";
    private const string EmailUsername = "dev@kontrave.rs";
    private const string EmailPassword = "1312kontra";
    private const string EmailHost = "mail.kontrave.rs";
    private const int EmailPort = 587;

    public async Task SendJobEmailAsync(Person receiver, Job job, CancellationToken cancellationToken)
    {
        using var smtpClient = new SmtpClient(EmailHost);
        smtpClient.Port = EmailPort;
        smtpClient.Credentials = new NetworkCredential(EmailUsername, EmailPassword);
        smtpClient.EnableSsl = true;
        smtpClient.Timeout = 10;

        var emailTemplate = ResourceReader.ReadTextLinesAsync(GjJobEmailTemplateHtml);
        
        var stringBuilder = new StringBuilder();
        foreach (var line in emailTemplate.Result)
        {
            stringBuilder.AppendLine(line);
        }
        
        stringBuilder.Replace("{{ Job Title }}", job.Title);
        stringBuilder.Replace("{{ Job Description }}", job.Description);
        stringBuilder.Replace("{{ Apply Link }}", job.Url);
        stringBuilder.Replace("{{ Time the job is published }}", job.PublishedAtUtc.ToString("f"));
        
        var emailContent = stringBuilder.ToString();

        var mailMessage = new MailMessage()
        {
            IsBodyHtml = true,
            From = new MailAddress(From),
            Body = emailContent,
            Subject = $"New Kontravers GoodJob - {job.Title}"
        };
        
        mailMessage.To.Add(receiver.Email);
        
        await smtpClient.SendMailAsync(mailMessage, cancellationToken);
    }
}