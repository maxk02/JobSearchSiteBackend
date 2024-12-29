using Core.Domains.Jobs;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class JobConfiguration : EntityConfigurationBase<Job>
{
    public override void Configure(EntityTypeBuilder<Job> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(job => job.Company)
            .WithMany(company => company.Jobs)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(job => job.JobFolders)
            .WithMany(jobFolder => jobFolder.Jobs);

        builder
            .HasMany(job => job.JobContractTypes)
            .WithMany(jobContractType => jobContractType.Jobs);

        builder
            .HasOne(job => job.Category)
            .WithMany(category => category.Jobs)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(job => job.Locations)
            .WithMany(location => location.Jobs);

        builder
            .HasMany(job => job.JobApplications)
            .WithOne(jobApplication => jobApplication.Job)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(job => job.UsersWhoBookmarked)
            .WithMany(user => user.BookmarkedJobs)
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("JobBookmarks"));
        
        builder.OwnsOne(job => job.SalaryRecord,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        builder.OwnsMany(job => job.Responsibilities,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        builder.OwnsMany(job => job.Requirements,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        builder.OwnsMany(job => job.Advantages,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        builder.OwnsOne(job => job.EmploymentTypeRecord,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
    }
}