using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domains.JobApplications.Persistence;

public class JobApplicationTagConfiguration : IEntityTypeConfiguration<JobApplicationTag>
{
    public void Configure(EntityTypeBuilder<JobApplicationTag> builder)
    {
        builder.HasKey(jobApplication => jobApplication.Id);
        
        builder
            .HasIndex(jobApplicationTag => new { jobApplicationTag.JobApplicationId, jobApplicationTag.Tag })
            .IsUnique();
        
        builder
            .HasOne(jobApplicationTag => jobApplicationTag.JobApplication)
            .WithMany(jobApplication => jobApplication.Tags)
            .HasForeignKey(tag => tag.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}