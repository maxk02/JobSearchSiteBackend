using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Cvs.UseCases.GetCvById;

public record GetCvByIdRequest(long CvId) : IRequest<Result<GetCvByIdResponse>>;