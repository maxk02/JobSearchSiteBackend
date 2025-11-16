using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearchSiteBackend.Core.Domains.Companies.Persistence;

public class CompanyBalanceTransactionConfiguration : IEntityTypeConfiguration<CompanyBalanceTransaction>
{
    public void Configure(EntityTypeBuilder<CompanyBalanceTransaction> builder)
    {
        builder.HasKey(companyBalanceTransaction => companyBalanceTransaction.Id);
        
        builder.HasIndex(companyBalanceTransaction => companyBalanceTransaction.GuidIdentifier).IsUnique();

        builder
            .HasOne(companyBalanceTransaction => companyBalanceTransaction.Company)
            .WithMany(company => company.CompanyBalanceTransactions)
            .HasForeignKey(companyBalanceTransaction => companyBalanceTransaction.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne(companyBalanceTransaction => companyBalanceTransaction.UserProfile)
            .WithMany(userProfile => userProfile.CompanyBalanceTransactions)
            .HasForeignKey(companyBalanceTransaction => companyBalanceTransaction.UserProfileId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}