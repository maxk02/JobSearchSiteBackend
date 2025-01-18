using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Cvs.UseCases.DeleteCv;

public record DeleteCvRequest(long CvId) : IRequest<Result>;