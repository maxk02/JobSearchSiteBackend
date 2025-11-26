using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Persistence;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasKey(job => job.Id);

        builder
            .HasOne(job => job.JobFolder)
            .WithMany(jobFolder => jobFolder.Jobs)
            .HasForeignKey(job => job.JobFolderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(job => job.JobContractTypes)
            .WithMany(jobContractType => jobContractType.Jobs);

        builder
            .HasOne(job => job.Category)
            .WithMany(category => category.Jobs)
            .HasForeignKey(job => job.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(job => job.Locations)
            .WithMany(location => location.Jobs);

        builder
            .HasMany(job => job.JobApplications)
            .WithOne(jobApplication => jobApplication.Job)
            .HasForeignKey(jobApplication => jobApplication.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(job => job.UsersWhoBookmarked)
            .WithMany(user => user.BookmarkedJobs)
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("JobBookmarks"));

        builder
            .HasOne(job => job.SalaryInfo)
            .WithOne()
            .HasForeignKey<JobSalaryInfo>(salaryInfo => salaryInfo.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(job => job.EmploymentOptions)
            .WithMany(employmentType => employmentType.Jobs);
        
        builder.Property(job => job.Responsibilities).HasColumnType("nvarchar(max)");
        
        builder.Property(job => job.Requirements).HasColumnType("nvarchar(max)");
        
        builder.Property(job => job.NiceToHaves).HasColumnType("nvarchar(max)");
    }
}