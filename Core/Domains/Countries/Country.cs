using Core.Domains._Shared.Entities;
using Core.Domains.Companies;
using Core.Domains.JobContractTypes;
using Core.Domains.Locations;

namespace Core.Domains.Countries;

public class Country : BaseEntity
{
    public static class SeededValues
    {
        public static readonly Country Poland = new Country(1, "POL");
        public static readonly Country Germany = new Country(2, "DEU");
        public static readonly Country France = new Country(3, "FRA");
    }
    
    private Country(long id, string code) : base(id)
    {
        Code = code;
    }
    
    public string Code { get; private set; }
    
    public virtual ICollection<JobContractType>? JobContractTypes { get; set; }
    public virtual ICollection<Location>? Locations { get; set; }
    public virtual ICollection<Company>? Companies { get; set; }
}