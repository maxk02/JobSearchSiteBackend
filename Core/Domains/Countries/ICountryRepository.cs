namespace Core.Domains.Countries;

public interface ICountryRepository
{
    public Task<ICollection<Country>> GetAll(CancellationToken cancellationToken = default);
}