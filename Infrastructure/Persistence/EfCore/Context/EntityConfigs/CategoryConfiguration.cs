using Core.Domains.Categories;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class CategoryConfiguration : MyBaseEntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);
        
        builder
            .HasMany(category => category.Children)
            .WithOne(childCategory => childCategory.Parent)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasMany(category => category.Cvs)
            .WithMany(cv => cv.Categories);
        
        builder
            .HasMany(category => category.Jobs)
            .WithOne(job => job.Category)
            .OnDelete(DeleteBehavior.Restrict);
    }
}