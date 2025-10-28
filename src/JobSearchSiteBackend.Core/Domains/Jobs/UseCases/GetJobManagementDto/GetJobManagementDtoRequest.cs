using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobManagementDto;

public record GetJobManagementDtoRequest(long Id) : IRequest<Result<GetJobManagementDtoResponse>>;