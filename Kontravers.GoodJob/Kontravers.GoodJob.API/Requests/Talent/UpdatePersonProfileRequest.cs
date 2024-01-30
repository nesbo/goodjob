using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Talent.Commands;

namespace Kontravers.GoodJob.API.Requests.Talent;

public class UpdatePersonProfileRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Skills { get; set; }

    public UpdatePersonProfileCommand ToCommand(IClock clock, string personId, string profileId)
    {
        return new(clock.UtcNow, Title, Description, Skills, personId, profileId);
    }
}