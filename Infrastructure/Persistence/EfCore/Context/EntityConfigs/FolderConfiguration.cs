using Core.Domains.JobFolders;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class FolderConfiguration : MyBaseEntityConfiguration<JobFolder>
{
    public override void Configure(EntityTypeBuilder<JobFolder> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(folder => folder.Company)
            .WithMany(company => company.JobFolders)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(folder => folder.Parent)
            .WithMany(folder => folder.Children)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(folder => folder.Jobs)
            .WithMany(job => job.Folders);
    }
}