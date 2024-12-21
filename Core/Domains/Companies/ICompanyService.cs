using Core.Domains.Companies.UseCases.CreateCompany;
using Core.Domains.Companies.UseCases.GetCompanyById;
using Core.Domains.Companies.UseCases.UpdateCompany;
using Shared.Result;

namespace Core.Domains.Companies;

public interface ICompanyService
{
    public Task<Result<long>> CreateCompanyAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default);
    public Task<Result> DeleteCompanyAsync(long companyId, CancellationToken cancellationToken = default);
    public Task<Result> UpdateCompanyAsync(UpdateCompanyRequest request, CancellationToken cancellationToken = default);
    public Task<Result<GetCompanyByIdResponse>> GetCompanyByIdAsync(long companyId, CancellationToken cancellationToken = default);
}