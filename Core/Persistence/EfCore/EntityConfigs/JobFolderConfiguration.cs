using Core.Domains.JobFolders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class JobFolderConfiguration : IEntityTypeConfiguration<JobFolder>
{
    public void Configure(EntityTypeBuilder<JobFolder> builder)
    {
        builder.HasKey(jobFolder => jobFolder.Id);

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