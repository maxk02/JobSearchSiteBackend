using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domains.JobFolders.Persistence;

public class JobFolderRelationConfiguration : IEntityTypeConfiguration<JobFolderRelation>
{
    public void Configure(EntityTypeBuilder<JobFolderRelation> builder)
    {
        builder.HasKey(relation => new { relation.AncestorId, relation.DescendantId });

        builder.HasOne(relation => relation.Ancestor)
            .WithMany(jobFolder => jobFolder.RelationsWhereThisIsAncestor)
            .HasForeignKey(relation => relation.AncestorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(relation => relation.Descendant)
            .WithMany(jobFolder => jobFolder.RelationsWhereThisIsDescendant)
            .HasForeignKey(relation => relation.DescendantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(relation => relation.Depth).IsRequired();
    }
}