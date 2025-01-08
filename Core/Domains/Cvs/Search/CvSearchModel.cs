using Core.Domains._Shared.Search;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Cvs.ValueEntities;

namespace Core.Domains.Cvs.Search;

public record CvSearchModel(
    long Id, long UserId,
    ICollection<EducationRecord> EducationRecords,
    ICollection<WorkRecord> WorkRecords,
    ICollection<string> Skills,
    ICollection<long> JobIdsApplied,
    ICollection<long> JobIdsUnapplied
) : ISearchModel;