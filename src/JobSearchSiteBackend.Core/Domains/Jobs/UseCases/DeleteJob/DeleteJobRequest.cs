using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.DeleteJob;

public record DeleteJobRequest(long Id) : IRequest<Result>;