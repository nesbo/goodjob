using Kontravers.GoodJob.Domain.Talent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kontravers.GoodJob.Data.EntityConfigurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profile", ConfigHelper.TalentSchema)
            .HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnName("ProfileId");
        
        builder.SetRequired(p=> p.CreatedUtc,
            p=> p.InsertedUtc,
            p=> p.Title,
            p=> p.Description,
            p=> p.PersonId);

        builder.Property(p => p.Skills).HasMaxLength(256);
        builder.Property(p=> p.Title).HasMaxLength(256);
        builder.Property(p=> p.Description).HasMaxLength(4096);
    }
}