// using Core.Domains._Shared.Repositories;
// using Core.Domains._Shared.UnitOfWork;
// using Core.Domains._Shared.UseCaseStructure;
// using Core.Domains.PersonalFiles;
// using Core.Domains.PersonalFiles.Search;
// using Core.Persistence.EfCore;
// using Core.Services.Auth.Authentication;
// using Core.Services.QueueService;
// using Microsoft.EntityFrameworkCore;
// using Shared.Result;
//
// namespace Core.Domains.JobApplications.UseCases.UpdateApplicationFiles;
//
// public class UpdateApplicationFilesHandler(
//     ICurrentAccountService currentAccountService,
//     IPersonalFileSearchRepository personalFileSearchRepository,
//     IBackgroundJobQueueService jobQueueService,
//     MainDataContext context) : IRequestHandler<UpdateApplicationFilesRequest, Result>
// {
//     public async Task<Result> Handle(UpdateApplicationFilesRequest request,
//         CancellationToken cancellationToken = default)
//     {
//         var currentUserId = currentAccountService.GetIdOrThrow();
//         
//         // var jobApplication = await context.JobApplications.FindAsync([request.ApplicationId], cancellationToken);
//         var jobApplicationWithFileIdsQuery = 
//             from jobApplication in context.JobApplications
//             join personalFile in context.PersonalFiles on jobApplication.PersonalFiles equals personalFile.JobApplications into pfGroup 
//             from personalFile in pfGroup.DefaultIfEmpty()
//             group personalFile.Id by new { JobApplication = jobApplication } into grouped
//             select new { grouped.Key.JobApplication, grouped.ToList() };
//
//         if (jobApplication is null)
//             return Result.NotFound();
//
//         if (jobApplication.UserId != currentUserId)
//             return Result.Forbidden();
//         
//         var personalFiles = await context.PersonalFiles
//             .Where(pf => request.PersonalFileIds.Contains(pf.Id))
//             .ToListAsync(cancellationToken);
//         
//         if (!request.PersonalFileIds.All(personalFiles.Select(x => x.Id).Contains) ||
//             personalFiles.Any(x => x.UserId != currentUserId))
//         {
//             return Result<long>.Error();
//         }
//         
//         
//         jobApplication.PersonalFiles = personalFiles;
//         context.Update(jobApplication);
//         await context.SaveChangesAsync(cancellationToken);
//         
//         
//         var jobId = jobApplication.JobId;
//         
//         try
//         {
//             await personalFileSearchRepository.AddAppliedToJobIdAsync(request.PersonalFileIds, jobId);
//         }
//         catch
//         {
//             await jobQueueService.EnqueueForIndefiniteRetriesAsync<IPersonalFileSearchRepository>(
//                 x => x.AddAppliedToJobIdAsync(request.PersonalFileIds, jobId));
//         }
//         
//
//         return Result.Success();
//     }
// }