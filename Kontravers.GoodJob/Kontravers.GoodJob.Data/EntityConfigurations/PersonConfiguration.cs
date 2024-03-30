using Kontravers.GoodJob.Domain.Talent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kontravers.GoodJob.Data.EntityConfigurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(c => c.Id)
            .HasColumnName("PersonId");
        builder.ToTable("Person", ConfigHelper.TalentSchema)
            .HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(128);
        builder.Property(p => p.Email)
            .HasMaxLength(128);
        builder.Property(p => p.UserId)
            .HasMaxLength(128)
            .HasDefaultValue("1");

        builder.HasMany(p => p.UpworkRssFeeds)
            .WithOne()
            .HasForeignKey(r=> r.PersonId);

        builder.HasMany(p => p.Profiles)
            .WithOne()
            .HasForeignKey(p => p.PersonId);

        builder.HasIndex(p=> new { p.Email, p.OrganisationId }).IsUnique();
        
        builder.SetRequired(p=> p.IsEnabled,
            p=> p.Name,
            p=> p.CreatedUtc,
            p=> p.InsertedUtc,
            p=> p.OrganisationId,
            p=> p.Email,
            p=> p.UserId);
    }
}