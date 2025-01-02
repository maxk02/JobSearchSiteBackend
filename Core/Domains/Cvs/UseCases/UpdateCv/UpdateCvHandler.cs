using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Categories;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Cvs.UseCases.UpdateCv;

public class UpdateCvHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateCvRequest, Result>
{
    public async Task<Result> Handle(UpdateCvRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var cv = await context.Cvs
            .Include(cv => cv.SalaryRecord)
            .Include(cv => cv.EmploymentTypeRecord)
            .Include(cv => cv.EducationRecords)
            .Include(cv => cv.WorkRecords)
            .FirstOrDefaultAsync(cv => cv.Id == request.CvId, cancellationToken);

        if (cv is null)
            return Result.NotFound();
        
        if (cv.UserId != currentUserId)
            return Result.Forbidden();

        var newCvCreationResult = Cv.Create(
            cv.UserId,
            request.SalaryRecord ?? cv.SalaryRecord,
            request.EmploymentTypeRecord ?? cv.EmploymentTypeRecord,
            request.EducationRecords ?? [..cv.EducationRecords],
            request.WorkRecords ?? [..cv.WorkRecords],
            request.Skills ?? [..cv.Skills]
            );
        
        if (newCvCreationResult.IsFailure)
            return newCvCreationResult;
        
        var newCv = newCvCreationResult.Value;

        if (request.CategoryIds is not null)
        {
            newCv.Categories = Category.AllValues
                .Where(c => request.CategoryIds.Contains(c.Id))
                .ToList();
        }

        context.Cvs.Update(newCv);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}