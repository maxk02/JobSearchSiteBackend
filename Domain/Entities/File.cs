using System.Collections;
using Domain.Common;

namespace Domain.Entities;

public class File : BaseEntity
{
    public string Name { get; set; } = "";
    public string FileType { get; set; } = "";
    
    public virtual IList<UserDataSet>? Users { get; set; }
    public virtual IList<Application>? Applications { get; set; }
}