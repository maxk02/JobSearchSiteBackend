using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.DeleteJob;

public record DeleteJobCommand(long Id) : IRequest<Result>;