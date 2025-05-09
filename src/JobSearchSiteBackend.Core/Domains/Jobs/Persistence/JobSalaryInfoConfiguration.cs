using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Persistence;

public class JobSalaryInfoConfiguration : IEntityTypeConfiguration<JobSalaryInfo>
{
    public void Configure(EntityTypeBuilder<JobSalaryInfo> builder)
    {
        builder.HasKey(jobSalaryInfo => jobSalaryInfo.JobId);
        
        builder
            .HasOne<Job>()
            .WithOne(job => job.SalaryInfo)
            .HasForeignKey<JobSalaryInfo>(salaryInfo => salaryInfo.JobId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .Property(jobSalaryInfo => jobSalaryInfo.Minimum)
            .HasColumnType("decimal(12,2)");
        
        builder
            .Property(jobSalaryInfo => jobSalaryInfo.Maximum)
            .HasColumnType("decimal(12,2)");
    }
}