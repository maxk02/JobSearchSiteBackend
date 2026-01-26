using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.Companies.Persistence;

public class CompanyEmployeeInvitationConfiguration : IEntityTypeConfiguration<CompanyEmployeeInvitation>
{
    public void Configure(EntityTypeBuilder<CompanyEmployeeInvitation> builder)
    {
        builder.HasKey(companyEmployeeInvitation => companyEmployeeInvitation.Id);
        
        builder.HasIndex(companyEmployeeInvitation => companyEmployeeInvitation.GuidIdentifier).IsUnique();

        builder
            .HasOne(companyEmployeeInvitation => companyEmployeeInvitation.Company)
            .WithMany(company => company.CompanyEmployeeInvitations)
            .HasForeignKey(companyEmployeeInvitation => companyEmployeeInvitation.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(companyEmployeeInvitation => companyEmployeeInvitation.InvitedUser)
            .WithMany(userProfile => userProfile.CompanyEmployeeInvitationsReceived)
            .HasForeignKey(companyEmployeeInvitation => companyEmployeeInvitation.InvitedUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(companyEmployeeInvitation => companyEmployeeInvitation.SenderUser)
            .WithMany(userProfile => userProfile.CompanyEmployeeInvitationsSent)
            .HasForeignKey(companyEmployeeInvitation => companyEmployeeInvitation.SenderUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}