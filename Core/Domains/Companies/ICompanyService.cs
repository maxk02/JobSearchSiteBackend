using Core.Domains.Companies.UseCases.AddCompany;
using Shared.Result;

namespace Core.Domains.Companies;

public interface ICompanyService
{
    public Task<Result<long>> AddCompanyAsync(AddCompanyRequest request, CancellationToken cancellationToken = default);
}