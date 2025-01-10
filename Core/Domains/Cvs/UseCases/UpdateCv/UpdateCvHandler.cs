using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Categories;
using Core.Domains.Cvs.Search;
using Core.Domains.Cvs.ValueEntities;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Cvs.UseCases.UpdateCv;

public class UpdateCvHandler(
    ICurrentAccountService currentAccountService,
    ICvSearchRepository cvSearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<UpdateCvRequest, Result>
{
    public async Task<Result> Handle(UpdateCvRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var cv = await context.Cvs
            .FirstOrDefaultAsync(cv => cv.Id == request.CvId, cancellationToken);

        if (cv is null)
            return Result.NotFound();

        if (cv.UserId != currentUserId)
            return Result.Forbidden();
        
        if (request.SalaryRecord is not null) cv.SalaryRecord = request.SalaryRecord;
        if (request.EmploymentTypeRecord is not null) cv.EmploymentTypeRecord = request.EmploymentTypeRecord;
        if (request.EducationRecords is not null) cv.EducationRecords = request.EducationRecords;
        if (request.WorkRecords is not null) cv.WorkRecords = request.WorkRecords;
        if (request.Skills is not null) cv.Skills = request.Skills;

        if (request.CategoryIds is not null && request.CategoryIds.Count != 0)
        {
            if (request.CategoryIds.Count != request.CategoryIds.Distinct().Count())
                return Result.Invalid();

            var categoriesThatNotExist = request.CategoryIds.Except(Category.AllIds);

            if (categoriesThatNotExist.Any())
                return Result.Invalid();

            cv.Categories = Category.AllValues.Where(c => request.CategoryIds.Contains(c.Id)).ToList();
        }

        
        //todo
        var cvSearchModel = new CvSearchModel(cv.Id, cv.UserId,
            cv.EducationRecords ?? [],
            cv.WorkRecords ?? [],
            cv.Skills ?? [],
            [], []);

        context.Cvs.Update(cv);
        await context.SaveChangesAsync(cancellationToken);
        
        backgroundJobService.Enqueue(
            () => cvSearchRepository.UpdateAsync(cvSearchModel, CancellationToken.None),
            BackgroundJobQueues.CvSearch
        );
        
        return Result.Success();
    }
}