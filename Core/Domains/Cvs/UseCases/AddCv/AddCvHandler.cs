using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Categories;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Cvs.UseCases.AddCv;

public class AddCvHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<AddCvRequest, Result>
{
    public async Task<Result> Handle(AddCvRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (currentUserId != request.UserId)
            return Result.Forbidden();

        var newCvCreationResult = Cv.Create(
            request.UserId,
            request.SalaryRecord,
            request.EmploymentTypeRecord,
            request.EducationRecords,
            request.WorkRecords,
            request.Skills
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

        context.Cvs.Add(newCv);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}