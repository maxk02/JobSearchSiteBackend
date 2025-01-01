using Core.Domains.JobContractTypes;
using Core.Persistence.EfCore.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class JobContractTypeConfiguration : EntityConfigurationBase<JobContractType>
{
    public override void Configure(EntityTypeBuilder<JobContractType> builder)
    {
        base.Configure(builder);
        
        builder
            .HasMany(jobContractType => jobContractType.Jobs)
            .WithMany(job => job.JobContractTypes);

        builder
            .HasOne(jobContractType => jobContractType.Country)
            .WithMany(country => country.JobContractTypes)
            .OnDelete(DeleteBehavior.Restrict);
    }
}