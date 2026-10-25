using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;
using JobSearchSiteBackend.Main.Services.Auth;
using JobSearchSiteBackend.Main.Persistence;

namespace JobSearchSiteBackend.Main.Domains.PersonalFiles.UseCases.UpdateFile;

public class UpdateFileHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateFileCommand, Result>
{
    public async Task<Result> Handle(UpdateFileCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var personalFile = await context.PersonalFiles.FindAsync([command.Id], cancellationToken);

        if (personalFile is null)
            return Result.NotFound();
        
        if (personalFile.UserId != currentUserId)
            return Result.Forbidden();

        personalFile.Name = command.NewName;
        
        context.PersonalFiles.Update(personalFile);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}