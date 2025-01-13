using Core.Domains.PersonalFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.EfCore.EntityConfigs;

public class PersonalFileConfiguration : IEntityTypeConfiguration<PersonalFile>
{
    public void Configure(EntityTypeBuilder<PersonalFile> builder)
    {
        builder.HasKey(personalFile => personalFile.Id);

        builder
            .HasOne(personalFile => personalFile.User)
            .WithMany(user => user.PersonalFiles)
            .HasForeignKey(personalFile => personalFile.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(personalFile => personalFile.JobApplications)
            .WithMany(jobApplication => jobApplication.PersonalFiles);
    }
}