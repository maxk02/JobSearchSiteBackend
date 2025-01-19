using Core.Domains.Cvs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class CvConfiguration : IEntityTypeConfiguration<Cv>
{
    public void Configure(EntityTypeBuilder<Cv> builder)
    {
        builder.HasKey(cv => cv.Id);
        
        builder.Property(cv => cv.RowVersion).IsRowVersion();
        
        builder
            .HasOne(cv => cv.User)
            .WithMany(user => user.Cvs)
            .HasForeignKey(cv => cv.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(cv => cv.UserId)
            .IsUnique();

        builder
            .HasMany(cv => cv.Categories)
            .WithMany(category => category.Cvs);
        
        builder.OwnsOne(cv => cv.SalaryRecord,
            ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.Property(salaryRecord => salaryRecord.Minimum).HasPrecision(10, 2);
                ownedNavigationBuilder.Property(salaryRecord => salaryRecord.Maximum).HasPrecision(10, 2);
            });
        
        builder.OwnsMany(cv => cv.EducationRecords,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        builder.OwnsMany(cv => cv.WorkRecords,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        builder.Property(cv => cv.Skills).HasColumnType("nvarchar(max)");
        
        builder.OwnsOne(cv => cv.EmploymentTypeRecord,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
    }
}