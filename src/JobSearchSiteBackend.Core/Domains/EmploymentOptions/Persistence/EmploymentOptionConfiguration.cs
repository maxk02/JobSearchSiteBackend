using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.EmploymentOptions.Persistence;

public class EmploymentOptionConfiguration : IEntityTypeConfiguration<EmploymentOption>
{
    public void Configure(EntityTypeBuilder<EmploymentOption> builder)
    {
        builder.HasKey(employmentType => employmentType.Id);
        
        builder.HasIndex(employmentType => employmentType.NamePl).IsUnique();

        builder
            .HasMany(employmentType => employmentType.Jobs)
            .WithMany(job => job.EmploymentTypes);

        builder.HasData(EmploymentOption.AllValues);
    }
}