using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domains.JobApplications.Persistence;

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.HasKey(jobApplication => jobApplication.Id);

        builder
            .HasOne(jobApplication => jobApplication.User)
            .WithMany(user => user.JobApplications)
            .HasForeignKey(jobApplication => jobApplication.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(jobApplication => jobApplication.Job)
            .WithMany(job => job.JobApplications)
            .HasForeignKey(jobApplication => jobApplication.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(jobApplication => jobApplication.PersonalFiles)
            .WithMany(personalFile => personalFile.JobApplications);

        builder
            .HasMany(jobApplication => jobApplication.Tags)
            .WithOne(tag => tag.JobApplication)
            .HasForeignKey(tag => tag.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}