using Core.Domains.UserProfiles;
using Core.Persistence.EfCore.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class UserConfiguration : EntityConfigurationBase<UserProfile>
{
    public override void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        base.Configure(builder);
        
        builder
            .HasMany(user => user.JobApplications)
            .WithOne(jobApplication => jobApplication.User)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(user => user.PersonalFiles)
            .WithOne(personalFile => personalFile.User)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(user => user.Cvs)
            .WithOne(cv => cv.User)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(user => user.UserFolderPermissions)
            .WithOne(userFolderFolderPermission => userFolderFolderPermission.User)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(user => user.UserCompanyPermissions)
            .WithOne(userCompanyCompanyPermission => userCompanyCompanyPermission.User)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasMany(user => user.BookmarkedJobs)
            .WithMany(job => job.UsersWhoBookmarked)
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("JobBookmarks"));
        
        builder
            .HasMany(user => user.BookmarkedCompanies)
            .WithMany(company => company.UsersWhoBookmarked)
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("CompanyBookmarks"));
    }
}