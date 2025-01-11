using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.CompanyClaims;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;

public class GetJobFolderClaimIdsForUserHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetJobFolderClaimIdsForUserRequest, Result<ICollection<long>>>
{
    public async Task<Result<ICollection<long>>> Handle(GetJobFolderClaimIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (currentUserId != request.UserId)
        {
            //todo hierarchy
            var isAdmin = await context.UserJobFolderClaims
                .AnyAsync(ujfp => ujfp.UserId == currentUserId 
                          && ujfp.FolderId == request.FolderId
                          && ujfp.ClaimId == CompanyClaim.IsAdmin.Id, cancellationToken);
            
            if (!isAdmin) 
                return Result<ICollection<long>>.Forbidden();
        }
        
        var permissionIds = await context.UserJobFolderClaims
            .Where(ujfp => ujfp.UserId == request.UserId && ujfp.FolderId == request.FolderId)
            .Select(ujfp => ujfp.ClaimId)
            .ToListAsync(cancellationToken);

        return Result<ICollection<long>>.Success(permissionIds);
    }
}