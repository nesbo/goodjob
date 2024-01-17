using Kontravers.GoodJob.Domain.Talent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kontravers.GoodJob.Data.EntityConfigurations;

public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.ToTable("Organisation", ConfigHelper.TalentSchema)
            .HasKey(p => p.Id);
        builder.Property(p=> p.Id)
            .HasColumnName("OrganisationId");
        
        builder.SetRequired(o=> o.Name,
            o=> o.CreatedUtc,
            o=> o.InsertedUtc);
        
        builder.Property(o=> o.Name)
            .HasMaxLength(128);
        builder.Property(o=> o.Description)
            .HasMaxLength(1024);
    }
}