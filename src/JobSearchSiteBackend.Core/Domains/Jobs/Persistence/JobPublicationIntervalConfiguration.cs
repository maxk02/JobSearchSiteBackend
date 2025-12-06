using JobSearchSiteBackend.Core.Domains.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Persistence;

public class JobPublicationIntervalConfiguration : IEntityTypeConfiguration<JobPublicationInterval>
{
    public void Configure(EntityTypeBuilder<JobPublicationInterval> builder)
    {
        builder.HasKey(jobPublicationInterval => jobPublicationInterval.Id);
        
        builder
            .HasOne<CountryCurrency>(jobPublicationInterval => jobPublicationInterval.CountryCurrency)
            .WithMany(cc => cc.JobPublicationIntervals)
            .HasForeignKey(jobPublicationInterval => jobPublicationInterval.CountryCurrencyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .Property(jobPublicationInterval => jobPublicationInterval.Price)
            .HasColumnType("decimal(10,2)");
    }
}