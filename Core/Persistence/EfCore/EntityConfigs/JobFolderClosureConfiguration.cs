using Core.Domains._Shared.Entities;
using Core.Domains.JobFolders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class JobFolderClosureConfiguration : IEntityTypeConfiguration<Closure<JobFolder>>
{
    public void Configure(EntityTypeBuilder<Closure<JobFolder>> builder)
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