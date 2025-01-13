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
            .HasForeignKey(jobApplication => jobApplication.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(userProfile => userProfile.PersonalFiles)
            .WithOne(personalFile => personalFile.User)
            .HasForeignKey(personalFile => personalFile.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(userProfile => userProfile.Cvs)
            .WithOne(cv => cv.User)
            .HasForeignKey(cv => cv.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(userProfile => userProfile.UserJobFolderClaims)
            .WithOne(userJobFolderClaim => userJobFolderClaim.User)
            .HasForeignKey(userJobFolderClaim => userJobFolderClaim.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(userProfile => userProfile.UserCompanyClaims)
            .WithOne(userCompanyClaim => userCompanyClaim.User)
            .HasForeignKey(userCompanyClaim => userCompanyClaim.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
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