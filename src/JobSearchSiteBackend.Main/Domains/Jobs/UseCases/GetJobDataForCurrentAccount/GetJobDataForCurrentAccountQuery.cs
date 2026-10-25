using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJob;

public record GetJobDataForCurrentAccountQuery(long JobId) : IRequest<Result<GetJobDataForCurrentAccountResult>>;