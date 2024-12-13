using API.Domains.ContractTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Persistence.EfCore.Context.EntityConfigs;

public class ContractTypeConfiguration : IEntityTypeConfiguration<ContractType>
{
    public void Configure(EntityTypeBuilder<ContractType> builder)
    {
        builder
            .HasMany(contractType => contractType.Jobs)
            .WithMany(job => job.ContractTypes);

        builder
            .HasOne(contractType => contractType.Country)
            .WithMany(country => country.ContractTypes)
            .OnDelete(DeleteBehavior.Restrict);
    }
}