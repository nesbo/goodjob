using Kontravers.GoodJob.Domain.Talent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kontravers.GoodJob.Data.EntityConfigurations;

public class PersonUpworkRssFeedConfiguration : IEntityTypeConfiguration<PersonUpworkRssFeed>
{
    public void Configure(EntityTypeBuilder<PersonUpworkRssFeed> builder)
    {
        builder.ToTable("PersonUpworkRssFeed", ConfigHelper.TalentSchema)
            .HasKey(p => p.Id);
        builder.Property(p=> p.Id)
            .HasColumnName("PersonUpworkRssFeedId");
        
        builder.SetRequired(r=> r.PersonId,
            r=>r.RootUrl,
            r=> r.RelativeUrl,
            r=> r.LastFetchedAtUtc,
            r=> r.MinFetchIntervalInMinutes,
            r=> r.CreatedUtc,
            r=> r.InsertedUtc);
        
        builder.Property(r=> r.RootUrl)
            .HasMaxLength(128);
        builder.Property(r=> r.RelativeUrl)
            .HasMaxLength(512);
        
        builder.HasIndex(r=> new { r.PersonId, r.RootUrl, r.RelativeUrl }).IsUnique();
    }
}