using Core.Domains.JobContractTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class JobContractTypeConfiguration : IEntityTypeConfiguration<JobContractType>
{
    public void Configure(EntityTypeBuilder<JobContractType> builder)
    {
        builder.HasKey(jobContractType => jobContractType.Id);
        
        builder
            .HasMany(jobContractType => jobContractType.Jobs)
            .WithMany(job => job.JobContractTypes);

        builder
            .HasOne(jobContractType => jobContractType.Country)
            .WithMany(country => country.JobContractTypes)
            .OnDelete(DeleteBehavior.Restrict);
    }
}