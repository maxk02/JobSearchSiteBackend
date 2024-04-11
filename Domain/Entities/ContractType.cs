using System.Collections;
using Domain.Common;

namespace Domain.Entities;

public class ContractType : BaseEntity
{
    public string Name { get; set; } = "";
    
    public long CountryId { get; set; }
    public virtual Country? Country { get; set; }
    
    public virtual IList<Job>? Jobs { get; set; }
}