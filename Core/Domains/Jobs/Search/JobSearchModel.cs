using Core.Domains._Shared.Search;

namespace Core.Domains.Jobs.Search;

public record JobSearchModel : ISearchModel
{
    public JobSearchModel(long id, byte[] version, long? jobFolderId = null, long? countryId = null,
        long? companyId = null, long? categoryId = null, string? title = null, string? description = null,
        ICollection<string>? responsibilities = null, ICollection<string>? requirements = null,
        ICollection<string>? advantages = null)
    {
        Id = id;
        Version = version;
        JobFolderId = jobFolderId;
        CountryId = countryId;
        CompanyId = companyId;
        CategoryId = categoryId;
        Title = title;
        Description = description;
        Responsibilities = responsibilities;
        Requirements = requirements;
        Advantages = advantages;
    }
    
    public long Id { get; init; }
    public byte[] Version { get; init; }
    public long? JobFolderId { get; set; }
    public long? CountryId { get; set; }
    public long? CompanyId { get; set; }
    public long? CategoryId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ICollection<string>? Responsibilities { get; set; }
    public ICollection<string>? Requirements { get; set; }
    public ICollection<string>? Advantages { get; set; }
}