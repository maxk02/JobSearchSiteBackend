using Domain.Entities.JobApplications;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class JobApplicationConfiguration : MyBaseEntityConfiguration<JobApplication>
{
    public override void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(jobApplication => jobApplication.User)
            .WithMany(user => user.JobApplications)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(jobApplication => jobApplication.Job)
            .WithMany(job => job.JobApplications)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(jobApplication => jobApplication.PersonalFiles)
            .WithMany(personalFile => personalFile.JobApplications);
    }
}