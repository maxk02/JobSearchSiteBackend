using API.Domains._Shared.EntityInterfaces;
using API.Domains.Companies;
using API.Domains.ContractTypes;
using API.Domains.Locations;

namespace API.Domains.Countries;

public class Country : IEntityWithId
{
    public static class SeededValues
    {
        public static readonly Country Poland = new Country(1, "POL");
        public static readonly Country Germany = new Country(2, "DEU");
        public static readonly Country France = new Country(3, "FRA");
    }
    
    private Country(long id, string code)
    {
        Id = id;
        Code = code;
    }
    
    public long Id { get; }
    
    public string Code { get; private set; }
    
    public virtual ICollection<ContractType>? ContractTypes { get; set; }
    public virtual ICollection<Location>? Locations { get; set; }
    public virtual ICollection<Company>? Companies { get; set; }
}