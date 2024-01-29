namespace Kontravers.GoodJob.Domain.Talent.Queries;

public class PersonProfileViewModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Skills { get; set; }
    public DateTime CreatedUtc { get; set; }
}