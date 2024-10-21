using Domain.Entities.Applications;
using Domain.Shared.Entities;

namespace Domain.Entities.Fileinfos;

public class Fileinfo : BaseEntity
{
    private string _name = null!;
    private string _extension = null!;
    private int _size;

    public required string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be empty");
            }
            if (value.Length > 50)
            {
                throw new ArgumentException("Too long string");
            }
            _name = value;
        }
    }
    public required string Extension
    {
        get => _extension;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be empty");
            }
            if (value.Length > 10)
            {
                throw new ArgumentException("Too long string");
            }
            _extension = value;
        }
    }
    public required int Size
    { 
        get => _size;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Incorrect file size"); 
            }
            if (value > 30000000) // 30 mb
            {
                throw new ArgumentException("Too large size"); 
            }
            _size = value;
        }
    }
    
    public byte[] Content { get; set; } = [];
    
    public virtual IList<User>? Users { get; set; }
    public virtual IList<Application>? MyApplications { get; set; }
}