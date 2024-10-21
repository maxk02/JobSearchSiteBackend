using Domain.Entities.Jobs;
using Domain.Shared.Entities;

namespace Domain.Entities.Users;

public class UserJobBookmark : BaseEntity
{
    private int _userId;
    private int _jobId;
    
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
    
    public virtual Job? Job { get; set; }
    public required int JobId
    {
        get => _jobId;
        set
        {
            if (_jobId < 1)
            {
                throw new ArgumentException("Value cannot be empty");
            }
            _jobId = value;
        }
    }
}