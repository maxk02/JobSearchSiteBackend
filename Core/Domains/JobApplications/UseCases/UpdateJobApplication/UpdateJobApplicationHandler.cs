using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.UpdateJobApplication;

public class UpdateJobApplicationHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateJobApplicationRequest, Result>
{
    public async Task<Result> Handle(UpdateJobApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobApplication = await context.JobApplications.FindAsync([request.JobApplicationId], cancellationToken);

        if (jobApplication is null)
            return Result.NotFound();
        
        //todo 
        
        jobApplication.Status = request.Status;

        context.JobApplications.Update(jobApplication);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}