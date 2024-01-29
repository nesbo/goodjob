using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Work;
using Kontravers.GoodJob.Infra.Shared;
using Kontravers.GoodJob.Tests.Fakes;

namespace Kontravers.GoodJob.Tests;

[TestFixture]
public class EmailSenderTest
{
    // [Test]
    // public async Task SendEmail()
    // {
    //     var job = new Job(1, "Job title", "url", "description", DateTime.UtcNow,
    //         Guid.NewGuid().ToString(),
    //         DateTime.UtcNow, DateTime.UtcNow,
    //         JobSourceType.Upwork, null, 1);
    //
    //     var person = new Person(true, "ilija.sasic@gmail.com", "Ilija", 1,
    //         DateTime.UtcNow, DateTime.UtcNow);
    //
    //     var emailSender = new EmailSender(new FakeClock());
    //
    //     Assert.DoesNotThrowAsync(() => emailSender.SendJobEmailAsync(person, job, CancellationToken.None));
    // }
}