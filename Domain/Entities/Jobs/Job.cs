using Domain.Entities.Applications;
using Domain.Entities.Categories;
using Domain.Entities.Companies;
using Domain.Entities.ContractTypes;
using Domain.Entities.Locations;
using Domain.Entities.Tags;
using Domain.Entities.Users;
using Domain.Shared.Entities;
using Domain.ValueObjects;

namespace Domain.Entities.Jobs;

public class Job : BaseEntity, IHideableEntity
{
    private string _title = null!; //possible null assignment and other validations handled in Name set accessor
    private int _companyId;
    private int _categoryId;
    private string _description = null!; //possible null assignment and other validations handled in Name set accessor
    
    public virtual Company? Company { get; set; }
    public required int CompanyId
    {
        get => _companyId;
        set
        {
            if (_companyId < 1)
            {
                throw new ArgumentException("Value cannot be empty");
            }
            _companyId = value;
        }
    }
    
    public virtual Category? Category { get; set; }
    public required int CategoryId
    {
        get => _categoryId;
        set
        {
            if (_categoryId < 1)
            {
                throw new ArgumentException("Value cannot be empty");
            }
            _categoryId = value;
        }
    }
    
    public required string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be empty");
            }
            _title = value;
        }
    }

    public required string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be empty");
            }
            _description = value;
        }
    }
    
    
    public required DateTime DateTimeExpiringUtc { get; set; }
    public SalaryRecord? SalaryRecord { get; set; }
    
    public IList<string> Responsibilities { get; set; } = [];
    public IList<string> Requirements { get; set; } = [];
    public IList<string> Advantages { get; set; } = [];
    public IList<string> Addresses { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
    
    public required bool IsHidden { get; set; }
    public required bool IsExpired { get; init; }
    
    public virtual IList<Application>? MyApplications { get; set; }
    public virtual IList<ContractType>? ContractTypes { get; set; }
    public virtual IList<Tag>? Tags { get; set; }
    public virtual IList<Location>? Locations { get; set; }
    
    public virtual IList<UserJobBookmark>? UserJobBookmarks { get; set; }
}