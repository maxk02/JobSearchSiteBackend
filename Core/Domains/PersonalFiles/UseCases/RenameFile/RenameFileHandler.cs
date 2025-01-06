using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.PersonalFiles.UseCases.RenameFile;

public class RenameFileHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<RenameFileRequest, Result>
{
    public async Task<Result> Handle(RenameFileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var personalFile = await context.PersonalFiles.FindAsync([request.FileId], cancellationToken);

        if (personalFile is null)
            return Result.NotFound();
        
        if (personalFile.UserId != currentUserId)
            return Result.Forbidden();

        var setNameResult = personalFile.SetName(request.NewName);
        if (!setNameResult.IsSuccess)
            return setNameResult;
        
        context.PersonalFiles.Update(personalFile);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}