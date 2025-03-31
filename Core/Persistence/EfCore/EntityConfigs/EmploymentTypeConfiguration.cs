using Core.Domains.EmploymentTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class EmploymentTypeConfiguration : IEntityTypeConfiguration<EmploymentType>
{
    public void Configure(EntityTypeBuilder<EmploymentType> builder)
    {
        builder.HasKey(employmentType => employmentType.Id);
        
        builder.HasIndex(employmentType => employmentType.NameEng).IsUnique();

        builder
            .HasMany(employmentType => employmentType.Jobs)
            .WithMany(job => job.EmploymentTypes);
    }
}