using Domain.Users;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class UserConfiguration : MyBaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
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
            .HasMany(user => user.UserFolderFolderPermissions)
            .WithOne(userFolderFolderPermission => userFolderFolderPermission.User)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(user => user.UserCompanyCompanyPermissions)
            .WithOne(userCompanyCompanyPermission => userCompanyCompanyPermission.User)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasMany(user => user.BookmarkedJobs)
            .WithMany(job => job.UsersWhoBookmarked);
        
        builder
            .HasMany(user => user.BookmarkedCompanies)
            .WithMany(company => company.UsersWhoBookmarked);
    }
}