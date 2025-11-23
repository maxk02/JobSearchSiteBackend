using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobManagementDto;

public record GetJobManagementDtoQuery(long Id) : IRequest<Result<GetJobManagementDtoResult>>;