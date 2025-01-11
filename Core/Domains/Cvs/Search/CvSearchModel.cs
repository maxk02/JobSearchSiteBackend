using Core.Domains._Shared.Search;
using Core.Domains.Cvs.ValueEntities;

namespace Core.Domains.Cvs.Search;

public record CvSearchModel : ISearchModel
{
    public CvSearchModel(long id, byte[] version, long? userId = null,
        ICollection<EducationRecord>? educationRecords = null,
        ICollection<WorkRecord>? workRecords = null,
        ICollection<string>? skills = null)
    {
        Id = id;
        Version = version;
        UserId = userId;
        EducationRecords = educationRecords;
        WorkRecords = workRecords;
        Skills = skills;
    }
    
    public long Id { get; init; }
    public byte[] Version { get; init; }
    public long? UserId { get; set; }
    public ICollection<EducationRecord>? EducationRecords { get; set; }
    public ICollection<WorkRecord>? WorkRecords { get; set; }
    public ICollection<string>? Skills { get; set; }
    private ICollection<long>? JobIdsApplied { get; set; } = null;
    private ICollection<long>? JobIdsUnapplied { get; set; } = null;
}