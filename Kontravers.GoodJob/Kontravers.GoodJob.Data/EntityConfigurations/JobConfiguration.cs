using Kontravers.GoodJob.Domain.Work;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kontravers.GoodJob.Data.EntityConfigurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Job", ConfigHelper.WorkSchema)
            .HasKey(p => p.Id);
        builder.Property(p=> p.Id)
            .HasColumnName("JobId");

        builder.SetRequired(j => j.JobStashId,
            j => j.Title,
            j => j.Url,
            j => j.Description,
            j => j.PublishedAtUtc,
            j => j.Budget);

        builder.Property(j => j.Title).HasMaxLength(256);
        builder.Property(j => j.Url).HasMaxLength(512);
        builder.Property(j => j.Description).HasMaxLength(10000);
        builder.Property(j => j.Budget).HasMaxLength(64);
        builder.Property(j => j.Skills).HasMaxLength(512);

        builder.Property(j => j.Status).HasConversion<byte>();
    }
}