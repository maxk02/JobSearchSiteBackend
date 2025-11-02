using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.Persistence;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder
            .HasOne(userProfile => userProfile.Account)
            .WithOne(account => account.Profile)
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

        // builder
        //     .HasMany(userProfile => userProfile.Cvs)
        //     .WithOne(cv => cv.User)
        //     .HasForeignKey(cv => cv.UserId)
        //     .OnDelete(DeleteBehavior.Cascade);

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
        
        builder
            .HasMany(userProfile => userProfile.LastManagedJobs)
            .WithMany()
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("LastManagedJobs"));
        
        builder
            .HasMany(userProfile => userProfile.LastManagedJobFolders)
            .WithMany()
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("LastManagedJobFolders"));
        
        builder
            .HasMany(userProfile => userProfile.BookmarkedJobApplications)
            .WithMany()
            .UsingEntity(junctionEntityBuilder => junctionEntityBuilder.ToTable("JobApplicationBookmarks"));
        
        builder
            .HasMany(userProfile => userProfile.UserAvatars)
            .WithOne(userAvatar => userAvatar.UserProfile)
            .HasForeignKey(userAvatar => userAvatar.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}