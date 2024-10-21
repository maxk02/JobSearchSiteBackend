using Domain.Entities.Companies;
using Domain.Shared.Entities;

namespace Domain.Entities.Users;

public class UserCompanyBookmark : BaseEntity
{
    private int _userId;
    private int _companyId;
    
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
}