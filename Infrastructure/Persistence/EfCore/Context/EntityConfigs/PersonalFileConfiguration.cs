using Domain.PersonalFiles;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class PersonalFileConfiguration : MyBaseEntityConfiguration<PersonalFile>
{
    public override void Configure(EntityTypeBuilder<PersonalFile> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(personalFile => personalFile.User)
            .WithMany(user => user.PersonalFiles)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(personalFile => personalFile.JobApplications)
            .WithMany(jobApplication => jobApplication.PersonalFiles);
    }
}