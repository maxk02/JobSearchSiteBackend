using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJobManagementDto;

public record GetJobManagementDtoQuery(long Id) : IRequest<Result<GetJobManagementDtoResult>>;