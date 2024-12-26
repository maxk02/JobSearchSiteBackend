using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.DeleteApplication;

public record DeleteApplicationRequest(long Id) : IRequest<Result>;