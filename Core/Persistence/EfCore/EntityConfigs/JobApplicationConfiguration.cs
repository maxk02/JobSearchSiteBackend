using Core.Domains.JobApplications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

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
    }
}