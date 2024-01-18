using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Work;

namespace Kontravers.GoodJob.Domain;

public interface IEmailSender
{
    Task SendJobEmailAsync(Person receiver, Job job, CancellationToken cancellationToken);
}