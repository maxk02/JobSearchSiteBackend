using API.Domains.PersonalFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Persistence.EfCore.Context.EntityConfigs;

public class PersonalFileConfiguration : IEntityTypeConfiguration<PersonalFile>
{
    public void Configure(EntityTypeBuilder<PersonalFile> builder)
    {
        builder
            .HasOne(personalFile => personalFile.User)
            .WithMany(user => user.PersonalFiles)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(personalFile => personalFile.JobApplications)
            .WithMany(jobApplication => jobApplication.PersonalFiles);
    }
}