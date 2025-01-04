using Core.Domains._Shared.Search;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Cvs.ValueEntities;

namespace Core.Domains.Cvs.Search;

public record CvSearchModel(
    long Id, long UserId,
    IReadOnlyCollection<EducationRecord> EducationRecords,
    IReadOnlyCollection<WorkRecord> WorkRecords,
    IReadOnlyCollection<string> Skills,
    IReadOnlyCollection<long> AppliedToJobIds
) : ISearchModel;