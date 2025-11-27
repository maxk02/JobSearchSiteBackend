using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains._SystemEntities.EmailsToSend.Persistence;

public class EmailToSendConfiguration : IEntityTypeConfiguration<EmailToSend>
{
    public void Configure(EntityTypeBuilder<EmailToSend> builder)
    {
        builder.HasKey(emailToSend => emailToSend.Id);
        
        builder.HasIndex(emailToSend => emailToSend.GuidIdentifier).IsUnique();
    }
}