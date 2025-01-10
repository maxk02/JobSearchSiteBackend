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
        
        // builder.HasOne(closure => closure.Ancestor)
        //     .WithMany(jobFolder => jobFolder.ClosuresWhereThisIsAncestor)
        //     .HasForeignKey(closure => closure.AncestorId)
        //     .OnDelete(DeleteBehavior.Restrict);
        //
        // builder.HasOne(closure => closure.Descendant)
        //     .WithMany(jobFolder => jobFolder.ClosuresWhereThisIsDescendant)
        //     .HasForeignKey(closure => closure.DescendantId)
        //     .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasMany(jobFolder => jobFolder.ClosuresWhereThisIsAncestor)
            .WithOne(closure => closure.Ancestor)
            .HasForeignKey(closure => closure.AncestorId);
        
        builder
            .HasMany(jobFolder => jobFolder.ClosuresWhereThisIsDescendant)
            .WithOne(closure => closure.Descendant)
            .HasForeignKey(closure => closure.DescendantId);

        builder
            .HasMany(jobFolder => jobFolder.Jobs)
            .WithOne(job => job.JobFolder);
    }
}