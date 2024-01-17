using Kontravers.GoodJob.Domain.Talent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kontravers.GoodJob.Data.EntityConfigurations;

public class PersonEntityConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(c => c.Id)
            .HasColumnName("PersonId");
        builder.ToTable("Person")
            .HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(128);
        builder.Property(p => p.Email)
            .HasMaxLength(128);
        builder.Property(p => p.OrganisationId)
            .HasMaxLength(16);

        builder.HasMany(p => p.UpworkRssFeeds)
            .WithOne()
            .HasForeignKey(r=> r.PersonId);

        builder.SetRequired(p => p.Id);
        builder.SetRequired(p=> p.IsEnabled);
        builder.SetRequired(p=> p.Name);
        builder.SetRequired(p=> p.OrganisationId);
        builder.SetRequired(p=> p.Email);
    }
}