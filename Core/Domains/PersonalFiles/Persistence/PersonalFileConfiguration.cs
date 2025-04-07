using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domains.PersonalFiles.Persistence;

public class PersonalFileConfiguration : IEntityTypeConfiguration<PersonalFile>
{
    public void Configure(EntityTypeBuilder<PersonalFile> builder)
    {
        builder.HasKey(personalFile => personalFile.Id);
        
        builder.Property(personalFile => personalFile.RowVersion).IsRowVersion();

        builder.Property(personalFile => personalFile.Name).HasMaxLength(40);
        builder.Property(personalFile => personalFile.Extension).HasMaxLength(25);

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