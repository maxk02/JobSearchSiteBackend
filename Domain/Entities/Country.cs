using System.Collections;
using Domain.Common;

namespace Domain.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; } = "";
    
    public virtual IList<Location>? Locations { get; set; }
    public virtual IList<Company>? Companies { get; set; }
    public virtual IList<ContractType>? JobContractTypes { get; set; }
}