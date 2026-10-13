using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Search;

public record JobSearchModel
(
    long Id,
    long CountryId,
    long CategoryId,
    long CompanyId,
    string Title,
    string? Description,
    bool IsPublic,
    DateTime DateTimePublishedUtc,
    DateTime DateTimeExpiringUtc,
    ICollection<string> Responsibilities,
    ICollection<string> Requirements,
    ICollection<string> NiceToHaves,
    ICollection<long> EmploymentOptionIds,
    ICollection<long> JobContractTypeIds,
    ICollection<long> LocationIds,
    ICollection<long> ParentLocationIds,
    ICollection<long> ChildLocationIds,
    Guid VersionId,
    bool IsDeleted
) : ISearchModelWithId, IUpdatableSearchModel;