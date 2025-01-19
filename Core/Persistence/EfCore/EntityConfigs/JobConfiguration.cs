using Core.Domains.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasKey(job => job.Id);
        
        builder.Property(job => job.RowVersion).IsRowVersion();

        builder
            .HasOne(job => job.JobFolder)
            .WithMany(jobFolder => jobFolder.Jobs)
            .HasForeignKey(job => job.JobFolderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(job => job.JobContractTypes)
            .WithMany(jobContractType => jobContractType.Jobs);

        builder
            .HasOne(job => job.Category)
            .WithMany(category => category.Jobs)
            .HasForeignKey(job => job.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(job => job.Locations)
            .WithMany(location => location.Jobs);

        builder
            .HasMany(job => job.JobApplications)
            .WithOne(jobApplication => jobApplication.Job)
            .HasForeignKey(jobApplication => jobApplication.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(job => job.UsersWhoBookmarked)
            .WithMany(user => user.BookmarkedJobs)
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("JobBookmarks"));
        
        builder.OwnsOne(job => job.SalaryRecord,
            ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.Property(salaryRecord => salaryRecord.Minimum).HasPrecision(10, 2);
                ownedNavigationBuilder.Property(salaryRecord => salaryRecord.Maximum).HasPrecision(10, 2);
            });
        
        builder.Property(job => job.Responsibilities).HasColumnType("nvarchar(max)");
        
        builder.Property(job => job.Requirements).HasColumnType("nvarchar(max)");
        
        builder.Property(job => job.Advantages).HasColumnType("nvarchar(max)");
        
        builder.OwnsOne(job => job.EmploymentTypeRecord,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
    }
}