using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;
using Core.Domains.Jobs.Dtos;

namespace Core.Domains.Jobs.UseCases.UpdateJob;

public record UpdateJobRequest(long Id, UpdateJobDto Job) : IRequest<Result>;