using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth;
using Ardalis.Result;
using Core.Persistence;

namespace Core.Domains.PersonalFiles.UseCases.UpdateFile;

public class UpdateFileHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateFileRequest, Result>
{
    public async Task<Result> Handle(UpdateFileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var personalFile = await context.PersonalFiles.FindAsync([request.Id], cancellationToken);

        if (personalFile is null)
            return Result.NotFound();
        
        if (personalFile.UserId != currentUserId)
            return Result.Forbidden();

        personalFile.Name = request.NewName;
        
        context.PersonalFiles.Update(personalFile);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}