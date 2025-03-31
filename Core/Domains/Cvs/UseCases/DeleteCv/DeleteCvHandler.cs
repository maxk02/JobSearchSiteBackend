// using System.Transactions;
// using Core.Domains._Shared.UseCaseStructure;
// using Core.Domains.Cvs.Search;
// using Core.Persistence.EfCore;
// using Core.Services.Auth;
// using Core.Services.BackgroundJobs;
// using Microsoft.EntityFrameworkCore;
// using Ardalis.Result;
//
// namespace Core.Domains.Cvs.UseCases.DeleteCv;
//
// public class DeleteCvHandler(
//     ICurrentAccountService currentAccountService,
//     ICvSearchRepository cvSearchRepository,
//     IBackgroundJobService backgroundJobService,
//     MainDataContext context) : IRequestHandler<DeleteCvRequest, Result>
// {
//     public async Task<Result> Handle(DeleteCvRequest request, CancellationToken cancellationToken = default)
//     {
//         var currentUserId = currentAccountService.GetIdOrThrow();
//         
//         var cv = await context.Cvs
//             .Include(cv => cv.EducationRecords)
//             .Include(cv => cv.WorkRecords)
//             .Include(cv => cv.Skills)
//             .FirstOrDefaultAsync(cv => cv.Id == request.CvId, cancellationToken);
//
//         if (cv is null)
//             return Result.NotFound();
//
//         if (cv.UserId != currentUserId)
//             return Result.Forbidden();
//         
//         var cvRowVersion = cv.RowVersion;
//         
//         var cvSearchModel = new CvSearchModel(
//             cv.Id,
//             cv.EducationRecords!,
//             cv.WorkRecords!,
//             cv.Skills!
//         );
//         
//         context.Cvs.Remove(cv);
//         
//         using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
//         
//         await context.SaveChangesAsync(cancellationToken);
//         
//         backgroundJobService.Enqueue(
//             () => cvSearchRepository.SoftDeleteAsync(cvSearchModel, cvRowVersion, CancellationToken.None),
//             BackgroundJobQueues.CvSearch);
//         
//         transaction.Complete();
//         
//         return Result.Success();
//     }
// }