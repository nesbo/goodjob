using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Talent.Commands;
using Paramore.Brighter;

namespace Kontravers.GoodJob.API.Requests.Talent;

public class CreatePersonProfileRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? Skills { get; set; }

    public CreatePersonProfileCommand ToCommand(IClock clock, string userId)
    {
        return new CreatePersonProfileCommand(userId, Title, Description, Skills, clock.UtcNow);
    }
}