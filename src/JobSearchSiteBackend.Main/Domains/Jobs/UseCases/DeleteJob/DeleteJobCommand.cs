using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.DeleteJob;

public record DeleteJobCommand(long Id) : IRequest<Result>;