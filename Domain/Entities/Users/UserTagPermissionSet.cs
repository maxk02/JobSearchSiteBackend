using Domain.Entities.Companies;
using Domain.Shared.Entities;

namespace Domain.Entities.Users;

public class UserTagPermissionSet : BaseEntity
{
    private int _userId;
    private int _tagId;
    
    public virtual User? User { get; set; }
    public required int UserId
    {
        get => _userId;
        set
        {
            if (_userId < 1)
            {
                throw new ArgumentException("Value cannot be empty");
            }
            _userId = value;
        }
    }
    
    public virtual Company? Tag { get; set; }
    public required int TagId
    {
        get => _tagId;
        set
        {
            if (_tagId < 1)
            {
                throw new ArgumentException("Value cannot be empty");
            }
            _tagId = value;
        }
    }
    
    public bool CanManageApplications { get; set; }
    public bool CanCreateEditDeleteJobs { get; set; }
    public bool IsTagAdmin { get; set; }
    public bool IsTagOwner { get; set; }
}