using Core.Domains.JobContractTypes;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class ContractTypeConfiguration : MyBaseEntityConfiguration<JobContractType>
{
    public override void Configure(EntityTypeBuilder<JobContractType> builder)
    {
        base.Configure(builder);
        
        builder
            .HasMany(contractType => contractType.Jobs)
            .WithMany(job => job.JobContractTypes);

        builder
            .HasOne(contractType => contractType.Country)
            .WithMany(country => country.ContractTypes)
            .OnDelete(DeleteBehavior.Restrict);
    }
}