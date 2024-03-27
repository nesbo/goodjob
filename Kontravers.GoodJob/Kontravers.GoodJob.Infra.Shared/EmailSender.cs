using System.Net;
using System.Net.Mail;
using System.Text;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Work;

namespace Kontravers.GoodJob.Infra.Shared;

public class EmailSender : IEmailSender
{
    private readonly IClock _clock;
    private const string From = "dev@kontrave.rs";
    private const string FromDisplayName = "Kontravers GoodJob";
    private const string GjJobEmailTemplateHtml = "GJ-job-email-template_01.html";
    private const string GjJobEmailJobProposalTemplateHtml = "GJ-job-email-job-proposal-template.html";
    private const string EmailUsername = "apikey";
    private const string EmailPassword = "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";
    private const string EmailHost = "smtp.sendgrid.net";
    private const int EmailPort = 587;

    public EmailSender(IClock clock)
    {
        _clock = clock;
    }

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

        if (job.HasProposals)
        {
            var proposalTemplate = ResourceReader
                .ReadTextLinesAsync(GjJobEmailJobProposalTemplateHtml);
            var proposalStringBuilder = new StringBuilder();
            foreach (var line in proposalTemplate.Result)
            {
                proposalStringBuilder.AppendLine(line);
            }

            proposalStringBuilder.Replace("{{ Proposal text }}", job.JobProposals.First().Text);
            stringBuilder.Replace("###Proposals###", proposalStringBuilder.ToString());
        }
        else
        {
            stringBuilder.Replace("###Proposals###", "");
        }
        
        var emailContent = stringBuilder.ToString();

        var mailMessage = new MailMessage
        {
            IsBodyHtml = true,
            From = new MailAddress(From, FromDisplayName),
            Body = emailContent,
            Subject = $"{job.PublishedAtLocal.ToString("t")} - {job.Title}"
        };
        
        mailMessage.To.Add(receiver.Email);
        
        await smtpClient.SendMailAsync(mailMessage, cancellationToken);
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Task.CompletedTask;
    }
}