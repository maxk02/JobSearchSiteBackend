using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace Core.Domains.Cvs.UseCases.GetCvById;

public class GetCvByIdHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetCvByIdRequest, Result<GetCvByIdResponse>>
{
    public async Task<Result<GetCvByIdResponse>> Handle(GetCvByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var query = context.Cvs
            .Include(cv => cv.SalaryRecord)
            .Include(cv => cv.EmploymentTypeRecord)
            .Include(cv => cv.EducationRecords)
            .Include(cv => cv.WorkRecords)
            .Include(cv => cv.Categories)
            .Where(cv => cv.Id == request.CvId);
        
        var cv = await query.FirstOrDefaultAsync(cancellationToken);

        if (cv is null)
            return Result<GetCvByIdResponse>.NotFound();
        
        if (cv.UserId != currentUserId)
            return Result<GetCvByIdResponse>.Forbidden();

        var response = new GetCvByIdResponse(cv.Id, cv.SalaryRecord,
            cv.EmploymentTypeRecord,
            cv.EducationRecords ?? [],
            cv.WorkRecords ?? [],
            cv.Skills ?? [],
            cv.Categories?.Select(c => c.Id).ToList() ?? []);

        return response;
    }
}