using Core.Domains.JobFolders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class JobFolderClosureConfiguration : IEntityTypeConfiguration<JobFolderClosure>
{
    public void Configure(EntityTypeBuilder<JobFolderClosure> builder)
    {
        builder.HasKey(closure => new { closure.AncestorId, closure.DescendantId });

        builder.HasOne(closure => closure.Ancestor)
            .WithMany(jobFolder => jobFolder.Descendants)
            .HasForeignKey(closure => closure.AncestorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(closure => closure.Descendant)
            .WithMany(jobFolder => jobFolder.Ancestors)
            .HasForeignKey(closure => closure.DescendantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(closure => closure.Depth).IsRequired();
    }
}