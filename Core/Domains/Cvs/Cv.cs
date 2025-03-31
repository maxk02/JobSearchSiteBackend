// using Core.Domains._Shared.EntityInterfaces;
// using Core.Domains._Shared.ValueEntities;
// using Core.Domains.Categories;
// using Core.Domains.Cvs.ValueEntities;
// using Core.Domains.Locations;
// using Core.Domains.UserProfiles;
// using Ardalis.Result;
//
// namespace Core.Domains.Cvs;
//
// public class Cv : IEntityWithId, IEntityWithRowVersioning
// {
//     public Cv(long userId, SalaryRecord? salaryRecord, EmploymentTypeRecord? employmentTypeRecord,
//         ICollection<EducationRecord> educationRecords,
//         ICollection<WorkRecord> workRecords, ICollection<string> skills)
//     {
//         UserId = userId;
//         SalaryRecord = salaryRecord;
//         EmploymentTypeRecord = employmentTypeRecord;
//         EducationRecords = [..educationRecords];
//         WorkRecords = [..workRecords];
//         Skills = [..skills];
//     }
//     
//     // ef core
//     private Cv(long userId, ICollection<string> skills)
//     {
//         UserId = userId;
//         Skills = [..skills];
//     }
//     
//     public long Id { get; private set; }
//
//     public byte[] RowVersion { get; private set; } = [];
//     
//     public long UserId { get; private set; }
//     
//     public SalaryRecord? SalaryRecord { get; set; }
//
//     public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
//     
//     public ICollection<EducationRecord>? EducationRecords { get; set; }
//     
//     public ICollection<WorkRecord>? WorkRecords { get; set; }
//     
//     public ICollection<string>? Skills { get; set; }
//     
//     public bool IsPublic { get; set; }
//     
//     public UserProfile? User { get;  private set; }
//     public ICollection<Location>? Locations { get; set; }
//     public ICollection<Category>? Categories { get; set; }
// }