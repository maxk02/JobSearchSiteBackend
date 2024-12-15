using Core.Domains._Shared.Entities;
using Core.Domains.Companies;
using Core.Domains.ContractTypes;
using Core.Domains.Locations;

namespace Core.Domains.Countries;

public class Country : BaseEntity
{
    public static class SeededValues
    {
        public static readonly Country Poland = new Country(1, Guid.Parse("83dc60dd-fedd-44a7-a5ef-fd2ffee14de9"), "POL");
        public static readonly Country Germany = new Country(2, Guid.Parse("ad058bcf-c1db-43c1-96b5-d46006a0be05"), "DEU");
        public static readonly Country France = new Country(3, Guid.Parse("e15af2a9-a0aa-4ca0-ba0f-906b0eeab9c5"), "FRA");
    }
    
    private Country(long id, Guid guidIdentifier, string code) : base(id, guidIdentifier)
    {
        Code = code;
    }
    
    public string Code { get; private set; }
    
    public virtual ICollection<ContractType>? ContractTypes { get; set; }
    public virtual ICollection<Location>? Locations { get; set; }
    public virtual ICollection<Company>? Companies { get; set; }
}