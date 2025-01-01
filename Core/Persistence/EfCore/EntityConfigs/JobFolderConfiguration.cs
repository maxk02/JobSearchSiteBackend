using Core.Domains.JobFolders;
using Core.Persistence.EfCore.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class JobFolderConfiguration : EntityConfigurationBase<JobFolder>
{
    public override void Configure(EntityTypeBuilder<JobFolder> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(jobFolder => jobFolder.Company)
            .WithMany(company => company.JobFolders)
            .OnDelete(DeleteBehavior.Restrict);

        // builder
        //     .HasOne(jobFolder => jobFolder.Parent)
        //     .WithMany(jobFolder => jobFolder.Children)
        //     .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(jobFolder => jobFolder.Jobs)
            .WithMany(job => job.JobFolders);
    }
}