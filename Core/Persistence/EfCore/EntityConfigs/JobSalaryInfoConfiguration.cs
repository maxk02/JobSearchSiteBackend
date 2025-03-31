using Core.Domains.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

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
    }
}