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

        builder.SetRequired(
            j => j.Title,
            j => j.Url,
            j => j.Description,
            j => j.PublishedAtUtc,
            j => j.CreatedUtc,
            j => j.InsertedUtc,
            j => j.Status,
            j => j.Uuid,
            j => j.Source,
            j => j.PersonId,
            j => j.PersonFeedId);
        
        builder.Property(j => j.Title).HasMaxLength(256);
        builder.Property(j => j.Url).HasMaxLength(2048);
        builder.Property(j => j.Description).HasMaxLength(10000);
        builder.Property(j => j.Skills).HasMaxLength(512);
        builder.Property(j => j.Uuid).HasMaxLength(2048);
        builder.Property(j => j.PreferredProfileId).HasConversion<int?>();

        builder.Property(j => j.Status).HasConversion<string>().HasMaxLength(64);
        builder.Property(j => j.Source).HasConversion<string>().HasMaxLength(64);
        
        builder.HasIndex(j=> new { j.Uuid, j.PersonId }).IsUnique();
        
        builder.HasMany(j=> j.JobProposals)
            .WithOne()
            .HasForeignKey(jp=> jp.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}