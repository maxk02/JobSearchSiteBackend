using API.Domains.Folders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Persistence.EfCore.Context.EntityConfigs;

public class FolderConfiguration : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder
            .HasOne(folder => folder.Company)
            .WithMany(company => company.Folders)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(folder => folder.Parent)
            .WithMany(folder => folder.Children)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(folder => folder.Children)
            .WithOne(folder => folder.Parent)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(folder => folder.Jobs)
            .WithMany(job => job.Folders);
    }
}