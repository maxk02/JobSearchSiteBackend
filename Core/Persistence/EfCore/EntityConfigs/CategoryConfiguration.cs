using Core.Domains.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);
        
        builder
            .HasMany(category => category.Cvs)
            .WithMany(cv => cv.Categories);
        
        builder
            .HasMany(category => category.Jobs)
            .WithOne(job => job.Category)
            .OnDelete(DeleteBehavior.Restrict);
    }
}