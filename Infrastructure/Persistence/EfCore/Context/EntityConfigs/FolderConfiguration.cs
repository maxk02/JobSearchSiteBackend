using Core.Domains.Folders;
using Infrastructure.Persistence.EfCore.Context.EntityConfigs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Context.EntityConfigs;

public class FolderConfiguration : MyBaseEntityConfiguration<Folder>
{
    public override void Configure(EntityTypeBuilder<Folder> builder)
    {
        base.Configure(builder);

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