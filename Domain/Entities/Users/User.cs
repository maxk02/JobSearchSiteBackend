using Domain.Entities.Applications;
using Domain.Entities.Categories;
using Domain.Entities.Fileinfos;
using Domain.Entities.Locations;
using Domain.Entities.Users;
using Domain.Entities.Users.ValueEntities;
using Domain.Shared.Entities;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : BaseEntity, IHideableEntity
{
    private string _firstName = null!; //possible null assignment and other validations handled in Name set accessor
    private string? _middleName;
    private string _lastName = null!; //possible null assignment and other validations handled in Name set accessor
    private DateOnly? _dateOfBirth;
    private string _email = null!; //possible null assignment and other validations handled in Name set accessor
    private string? _phone;

    public required string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be empty");
            }

            if (value.Length > 25)
            {
                throw new ArgumentException("Too long string");
            }
            _firstName = value;
        }
    }
    public string? MiddleName
    {
        get => _middleName;
        set
        {
            if (value != null && value.Length > 25)
            {
                throw new ArgumentException("Too long string");
            }
            _middleName = value;
        }
    }
    public required string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be empty");
            }

            if (value.Length > 25)
            {
                throw new ArgumentException("Too long string");
            }
            _lastName = value;
        }
    }

    public DateOnly? DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value != null && value < new DateOnly(1920, 01, 01))
            {
                throw new ArgumentException("Wrong date");
            }
            if (value != null && value > new DateOnly(1920, 01, 01))
            {
                throw new ArgumentException("Wrong date");
            }
            _dateOfBirth = value;
        }
    }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    
    public SalaryRecord? SalaryRecord { get; set; }
    public IList<EducationRecord> EducationRecords { get; set; } = [];
    public IList<WorkRecord> WorkRecords { get; set; } = [];
    public IList<string> Skills { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsHidden { get; set; }
    
    public virtual IList<Location>? Locations { get; set; }
    public virtual IList<Category>? Categories { get; set; }
    public virtual IList<Fileinfo>? MyFiles { get; set; }
    public virtual IList<Application>? MyApplications { get; set; }
    
    public virtual IList<UserJobBookmark>? UserJobBookmarks { get; set; }
    
    public virtual IList<UserCompanyBookmark>? UserCompanyBookmarks { get; set; }
    public virtual IList<UserCompanyPermissionSet>? UserCompanyPermissionSets { get; set; }
    
    public virtual IList<UserTagPermissionSet>? UserTagPermissionSets { get; set; }
}