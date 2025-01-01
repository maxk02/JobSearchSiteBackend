using Core.Domains.PersonalFiles;
using Core.Persistence.EfCore.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class PersonalFileConfiguration : EntityConfigurationBase<PersonalFile>
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