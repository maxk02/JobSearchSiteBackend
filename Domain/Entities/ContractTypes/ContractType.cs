using Domain.Entities.Jobs;
using Domain.Shared.Entities;

namespace Domain.Entities.ContractTypes;

public class ContractType : BaseEntity
{
    public string Name { get; private set; } = "";
    
    public virtual IList<Job>? Jobs { get; set; }
}