using System.Collections;
using Domain.Common;

namespace Domain.Entities;

public class MyFile : BaseEntity
{
    public string Name { get; set; } = "";
    public string Extension { get; set; } = "";
    public string Size { get; set; } = "";
    public byte[] Content { get; set; } = [];
    
    public virtual IList<User>? Users { get; set; }
    public virtual IList<MyApplication>? Applications { get; set; }
}