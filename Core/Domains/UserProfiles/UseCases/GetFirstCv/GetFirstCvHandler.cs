using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Cvs;
using Core.Domains.Cvs.Dtos;
using Core.Domains.Cvs.ValueEntities;
using Core.Domains.PersonalFiles;
using Core.Domains.PersonalFiles.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace Core.Domains.UserProfiles.UseCases.GetFirstCv;

public class GetFirstCvHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) 
    : IRequestHandler<GetFirstCvRequest, Result<GetFirstCvResponse>>
{
    public async Task<Result<GetFirstCvResponse>> Handle(GetFirstCvRequest request,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();
        
        if (currentAccountId != request.UserId)
            return Result<GetFirstCvResponse>.Forbidden();
        
        var query = context.Cvs
            .Include(cv => cv.SalaryRecord)
            .Include(cv => cv.EmploymentTypeRecord)
            .Include(cv => cv.EducationRecords)
            .Include(cv => cv.WorkRecords)
            .Include(cv => cv.Categories)
            .Where(cv => cv.UserId == request.UserId);
        
        var cv = await query.SingleOrDefaultAsync(cancellationToken);
        
        if (cv is null)
            return Result<GetFirstCvResponse>.NotFound();

        var cvDto = new CvDto(cv.Id, cv.UserId,
            cv.SalaryRecord, cv.EmploymentTypeRecord,
            cv.EducationRecords ?? [],
            cv.WorkRecords ?? [],
            cv.Skills ?? []);
        
        var response = new GetFirstCvResponse(cvDto);

        return response;
    }
}