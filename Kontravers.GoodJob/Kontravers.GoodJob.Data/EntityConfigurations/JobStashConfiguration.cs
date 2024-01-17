using Kontravers.GoodJob.Domain.Work;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kontravers.GoodJob.Data.EntityConfigurations;

public class JobStashConfiguration : IEntityTypeConfiguration<JobStash>
{
    public void Configure(EntityTypeBuilder<JobStash> builder)
    {
        builder.ToTable("JobStash", ConfigHelper.WorkSchema)
            .HasKey(p => p.Id);
        builder.Property(p=> p.Id)
            .HasColumnName("JobStashId");

        builder.SetRequired(r => r.PersonId);
        builder.HasIndex(p => p.PersonId).IsUnique();
        
        builder.HasMany(j=> j.Jobs)
            .WithOne()
            .HasForeignKey(j=> j.JobStashId);
    }
}