using Core.ContractTypes;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class ContractTypeConfiguration : MyBaseEntityConfiguration<ContractType>
{
    public override void Configure(EntityTypeBuilder<ContractType> builder)
    {
        base.Configure(builder);
        
        builder
            .HasMany(contractType => contractType.Jobs)
            .WithMany(job => job.ContractTypes);

        builder
            .HasOne(contractType => contractType.Country)
            .WithMany(country => country.ContractTypes)
            .OnDelete(DeleteBehavior.Restrict);
    }
}