using Core.Domains._Shared.EntityInterfaces;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Categories;
using Core.Domains.Cvs.ValueEntities;
using Core.Domains.Locations;
using Core.Domains.UserProfiles;
using Shared.Result;

namespace Core.Domains.Cvs;

public class Cv : IEntityWithId
{
    public Cv(long userId, SalaryRecord? salaryRecord, EmploymentTypeRecord? employmentTypeRecord,
        ICollection<EducationRecord> educationRecords,
        ICollection<WorkRecord> workRecords, ICollection<string> skills)
    {
        UserId = userId;
        SalaryRecord = salaryRecord;
        EmploymentTypeRecord = employmentTypeRecord;
        EducationRecords = [..educationRecords];
        WorkRecords = [..workRecords];
        Skills = [..skills];
    }
    
    public long Id { get; set; }
    
    public long UserId { get; private set; }
    
    public SalaryRecord? SalaryRecord { get; private set; }

    public EmploymentTypeRecord? EmploymentTypeRecord { get; private set; }
    
    public ICollection<EducationRecord>? EducationRecords { get; private set; }
    
    public ICollection<WorkRecord>? WorkRecords { get; private set; }
    
    public ICollection<string>? Skills { get; private set; }
    
    public bool IsPublic { get; private set; }
    
    public virtual UserProfile? User { get; set; }
    public virtual ICollection<Location>? Locations { get; set; }
    public virtual ICollection<Category>? Categories { get; set; }
}