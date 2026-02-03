using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.Persistence;

public class JobApplicationTagConfiguration : IEntityTypeConfiguration<JobApplicationTag>
{
    public void Configure(EntityTypeBuilder<JobApplicationTag> builder)
    {
        builder
            .HasKey(jobApplicationTag => new { jobApplicationTag.JobApplicationId, jobApplicationTag.Tag });
        
        builder
            .HasOne(jobApplicationTag => jobApplicationTag.JobApplication)
            .WithMany(jobApplication => jobApplication.Tags)
            .HasForeignKey(tag => tag.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}