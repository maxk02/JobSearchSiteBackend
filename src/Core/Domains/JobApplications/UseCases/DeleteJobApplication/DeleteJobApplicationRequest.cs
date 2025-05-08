using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.JobApplications.UseCases.DeleteJobApplication;

public record DeleteJobApplicationRequest(long Id) : IRequest<Result>;