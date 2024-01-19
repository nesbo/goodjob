using Kontravers.GoodJob.Domain.Work;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kontravers.GoodJob.Data.EntityConfigurations;

public class JobProposalConfiguration : IEntityTypeConfiguration<JobProposal>
{
    public void Configure(EntityTypeBuilder<JobProposal> builder)
    { 
        builder.ToTable("JobProposal", ConfigHelper.WorkSchema)
            .HasKey(jp=> jp.Id);
        builder.Property(jp => jp.Id)
            .HasColumnName("JobProposalId");

        builder.SetRequired(jp => jp.JobId,
            jp => jp.GeneratorType,
            jp => jp.Text,
            jp => jp.CreatedUtc,
            jp => jp.InsertedUtc,
            j => j.PersonId);
        
        builder.Property(jp=> jp.Text)
            .HasMaxLength(10240);
        builder.Property(jp => jp.GeneratorType)
            .HasConversion<string>();
    }
}