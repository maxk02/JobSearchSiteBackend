using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.EmploymentOptions.Persistence;

public class EmploymentOptionConfiguration : IEntityTypeConfiguration<EmploymentOption>
{
    public void Configure(EntityTypeBuilder<EmploymentOption> builder)
    {
        builder.HasKey(employmentOption => employmentOption.Id);
        
        builder.HasIndex(employmentOption => employmentOption.NamePl).IsUnique();

        builder
            .HasMany(employmentOption => employmentOption.Jobs)
            .WithMany(job => job.EmploymentOptions);

        builder.HasData(EmploymentOption.AllValues);
    }
}