using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJob;

public record GetJobQuery(long Id) : IRequest<Result<GetJobResult>>;