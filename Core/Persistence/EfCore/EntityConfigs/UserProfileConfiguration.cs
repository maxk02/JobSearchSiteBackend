using Core.Domains.UserProfiles;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder
            .HasOne<MyIdentityUser>()
            .WithOne()
            .HasForeignKey<UserProfile>(userProfile => userProfile.Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(userProfile => userProfile.JobApplications)
            .WithOne(jobApplication => jobApplication.User)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(userProfile => userProfile.PersonalFiles)
            .WithOne(personalFile => personalFile.User)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(userProfile => userProfile.Cvs)
            .WithOne(cv => cv.User)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(userProfile => userProfile.UserFolderPermissions)
            .WithOne(userProfileFolderPermission => userProfileFolderPermission.User)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(userProfile => userProfile.UserCompanyPermissions)
            .WithOne(userProfileCompanyPermission => userProfileCompanyPermission.User)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasMany(userProfile => userProfile.BookmarkedJobs)
            .WithMany(job => job.UsersWhoBookmarked)
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("JobBookmarks"));
        
        builder
            .HasMany(userProfile => userProfile.BookmarkedCompanies)
            .WithMany(company => company.UsersWhoBookmarked)
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("CompanyBookmarks"));
    }
}