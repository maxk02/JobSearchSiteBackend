using Core.Domains.EmploymentOptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class EmploymentTypeConfiguration : IEntityTypeConfiguration<EmploymentOption>
{
    public void Configure(EntityTypeBuilder<EmploymentOption> builder)
    {
        builder.HasKey(employmentType => employmentType.Id);
        
        builder.HasIndex(employmentType => employmentType.NamePl).IsUnique();

        builder
            .HasMany(employmentType => employmentType.Jobs)
            .WithMany(job => job.EmploymentTypes);
    }
}