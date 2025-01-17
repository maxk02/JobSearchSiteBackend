using Core.Domains._Shared.Search;
using Core.Domains.Cvs.ValueEntities;

namespace Core.Domains.Cvs.Search;

public record CvSearchModel(
    long Id,
    ICollection<EducationRecord> EducationRecords,
    ICollection<WorkRecord> WorkRecords,
    ICollection<string> Skills,
    DateTime? DeletionDateTimeUtc = null
) : ISearchModelWithId, ISearchModelWithDeletionDateTime;