using Core.Domains.UserProfiles;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class CvConfiguration : MyBaseEntityConfiguration<Cv>
{
    public override void Configure(EntityTypeBuilder<Cv> builder)
    {
        base.Configure(builder);
        
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