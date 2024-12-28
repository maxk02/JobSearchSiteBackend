using Core.Domains.JobFolders;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class JobFolderConfiguration : MyBaseEntityConfiguration<JobFolder>
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