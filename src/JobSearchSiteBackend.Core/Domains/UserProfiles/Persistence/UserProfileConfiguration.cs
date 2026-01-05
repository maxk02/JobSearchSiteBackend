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

        builder
            .HasMany(userProfile => userProfile.UserCompanyClaims)
            .WithOne(userCompanyClaim => userCompanyClaim.User)
            .HasForeignKey(userCompanyClaim => userCompanyClaim.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(userProfile => userProfile.UserJobBookmarks)
            .WithOne(ujb => ujb.UserProfile)
            .HasForeignKey(ujb => ujb.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(userProfile => userProfile.UserJobApplicationBookmarks)
            .WithOne(ujb => ujb.UserProfile)
            .HasForeignKey(ujb => ujb.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasMany(userProfile => userProfile.UserAvatars)
            .WithOne(userAvatar => userAvatar.UserProfile)
            .HasForeignKey(userAvatar => userAvatar.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasMany(userProfile => userProfile.CompanyBalanceTransactions)
            .WithOne(companyBalanceTransaction => companyBalanceTransaction.UserProfile)
            .HasForeignKey(companyBalanceTransaction => companyBalanceTransaction.UserProfileId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}