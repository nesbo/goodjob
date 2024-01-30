using Kontravers.GoodJob.Domain.Talent.Commands;
using Paramore.Brighter;

namespace Kontravers.GoodJob.Domain.Talent.UseCases;

public class CreatePersonUpworkRssFeedCommandHandler: RequestHandlerAsync<CreatePersonUpworkRssFeedCommand>
{
    public override async Task<CreatePersonUpworkRssFeedCommand> HandleAsync(CreatePersonUpworkRssFeedCommand command,
        CancellationToken cancellationToken = new CancellationToken())
    {
        
        
        return await base.HandleAsync(command, cancellationToken);
    }
}