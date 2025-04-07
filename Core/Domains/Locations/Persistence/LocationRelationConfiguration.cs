using Core.Domains.JobFolders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domains.Locations.Persistence;

public class LocationRelationConfiguration : IEntityTypeConfiguration<LocationRelation>
{
    public void Configure(EntityTypeBuilder<LocationRelation> builder)
    {
        builder.HasKey(relation => new { relation.AncestorId, relation.DescendantId });

        builder.HasOne(relation => relation.Ancestor)
            .WithMany(location => location.RelationsWhereThisIsAncestor)
            .HasForeignKey(relation => relation.AncestorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(relation => relation.Descendant)
            .WithMany(location => location.RelationsWhereThisIsDescendant)
            .HasForeignKey(relation => relation.DescendantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(relation => relation.Depth).IsRequired();
    }
}