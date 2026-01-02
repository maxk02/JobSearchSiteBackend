using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.GetAccountData;

public record GetAccountDataQuery : IRequest<Result<GetAccountDataResult>>;