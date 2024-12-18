using Core.Domains.Companies.UseCases.CreateCompany;
using Shared.Result;

namespace Core.Domains.Companies;

public interface ICompanyService
{
    public Task<Result<long>> CreateCompanyAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default);
    public Task<Result> DeleteCompanyAsync(long companyId, CancellationToken cancellationToken = default);
}