using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.UserProfiles;

namespace JobSearchSiteBackend.Core.Domains.Companies;

public class CompanyBalanceTransaction : IEntityWithId, IEntityWithGuid
{
    public CompanyBalanceTransaction(long companyId, decimal amount, string description, long currencyId, long userId)
    {
        CompanyId = companyId;
        Amount = amount;
        Description = description;
        CurrencyId = currencyId;
        UserProfileId = userId;
    }
    
    public long Id { get; private set; }
    
    public Guid GuidIdentifier { get; private set; } = Guid.NewGuid();
    
    public DateTime DateTimeCommittedUtc { get; set; }
    
    public decimal Amount { get; private set; }
    
    public string Description { get; private set; }
    
    public long CurrencyId { get; private set; }
    
    public long? CompanyId { get; set; }
    public long? UserProfileId { get; set; }
    
    public Company? Company { get; set; }
    public UserProfile? UserProfile { get; set; }
}