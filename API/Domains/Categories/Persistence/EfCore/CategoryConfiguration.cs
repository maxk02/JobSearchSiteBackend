using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Domains.Categories.Persistence.EfCore;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
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