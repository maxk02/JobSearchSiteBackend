using Core.Domains.Cvs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class CvConfiguration : IEntityTypeConfiguration<Cv>
{
    public void Configure(EntityTypeBuilder<Cv> builder)
    {
        builder.HasKey(cv => cv.Id);
        
        builder
            .HasOne(cv => cv.User)
            .WithMany(user => user.Cvs)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(cv => cv.UserId)
            .IsUnique();

        builder
            .HasMany(cv => cv.Categories)
            .WithMany(category => category.Cvs);
        
        builder.OwnsOne(cv => cv.SalaryRecord,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        builder.OwnsMany(cv => cv.EducationRecords,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        builder.OwnsMany(cv => cv.WorkRecords,
            ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.OwnsMany(workRecord => workRecord.Responsibilities,
                    nestedOwnedNavigationBuilder => nestedOwnedNavigationBuilder.ToJson());
            });
        
        builder.OwnsMany(cv => cv.Skills,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        builder.OwnsOne(cv => cv.EmploymentTypeRecord,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
    }
}