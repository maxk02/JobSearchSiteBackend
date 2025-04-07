using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domains.JobFolders.Persistence;

public class JobFolderConfiguration : IEntityTypeConfiguration<JobFolder>
{
    public void Configure(EntityTypeBuilder<JobFolder> builder)
    {
        builder.HasKey(jobFolder => jobFolder.Id);

        builder
            .HasOne(jobFolder => jobFolder.Company)
            .WithMany(company => company.JobFolders)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(jobFolder => jobFolder.RelationsWhereThisIsAncestor)
            .WithOne(relation => relation.Ancestor)
            .HasForeignKey(relation => relation.AncestorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasMany(jobFolder => jobFolder.RelationsWhereThisIsDescendant)
            .WithOne(relation => relation.Descendant)
            .HasForeignKey(relation => relation.DescendantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(jobFolder => jobFolder.Jobs)
            .WithOne(job => job.JobFolder)
            .HasForeignKey(job => job.JobFolderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}