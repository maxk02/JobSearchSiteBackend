using Core.Domains._Shared.Entities;
using Core.Domains.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class CategoryClosureConfiguration : IEntityTypeConfiguration<Closure<Category>>
{
    public void Configure(EntityTypeBuilder<Closure<Category>> builder)
    {
        builder.HasKey(closure => new { closure.AncestorId, closure.DescendantId });

        builder.HasOne(closure => closure.Ancestor)
            .WithMany(category => category.Descendants)
            .HasForeignKey(closure => closure.AncestorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(closure => closure.Descendant)
            .WithMany(category => category.Ancestors)
            .HasForeignKey(closure => closure.DescendantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(closure => closure.Depth).IsRequired();
    }
}