using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domains.JobContractTypes.Persistence;

public class JobContractTypeConfiguration : IEntityTypeConfiguration<JobContractType>
{
    public void Configure(EntityTypeBuilder<JobContractType> builder)
    {
        builder.HasKey(jobContractType => jobContractType.Id);
        
        builder
            .HasIndex(jobContractType => new { jobContractType.CountryId, Name = jobContractType.NamePl })
            .IsUnique();
        
        builder
            .HasMany(jobContractType => jobContractType.Jobs)
            .WithMany(job => job.JobContractTypes);

        builder
            .HasOne(jobContractType => jobContractType.Country)
            .WithMany(country => country.JobContractTypes)
            .HasForeignKey(jobContractType => jobContractType.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(JobContractType.AllValues);
    }
}