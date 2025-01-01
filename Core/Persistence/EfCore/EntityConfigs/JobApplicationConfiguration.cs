using Core.Domains.JobApplications;
using Core.Persistence.EfCore.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class JobApplicationConfiguration : EntityConfigurationBase<JobApplication>
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