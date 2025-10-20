// using System.Transactions;
// using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
// using JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;
// using JobSearchSiteBackend.Core.Services.Auth;
// using JobSearchSiteBackend.Core.Services.BackgroundJobs;
// using JobSearchSiteBackend.Core.Services.FileStorage;
// using JobSearchSiteBackend.Core.Services.TextExtraction;
// using Microsoft.Extensions.Caching.Memory;
// using Ardalis.Result;
// using JobSearchSiteBackend.Core.Persistence;
//
// namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UploadFile;
//
// public class UploadFileHandler(
//     ICurrentAccountService currentAccountService,
//     IFileStorageService fileStorageService,
//     IBackgroundJobService backgroundJobService,
//     ITextExtractionService textExtractionService,
//     IPersonalFileSearchRepository personalFileSearchRepository,
//     IMemoryCache memoryCache,
//     MainDataContext context) : IRequestHandler<UploadFileRequest, Result>
// {
//     public async Task<Result> Handle(UploadFileRequest request, CancellationToken cancellationToken = default)
//     {
//         var currentUserId = currentAccountService.GetIdOrThrow();
//         
//         if (request.FormFile is null || request.FormFile.Length == 0)
//         {
//             return Result.Invalid();
//         }
//
//         using var memoryStream = new MemoryStream();
//         await request.FormFile.CopyToAsync(memoryStream, cancellationToken);
//         
//         var fileName = Path.GetFileNameWithoutExtension(request.FormFile.FileName);
//         var extension = Path.GetExtension(request.FormFile.FileName).TrimStart('.');
//         var newFile = new PersonalFile(currentUserId, fileName, extension, memoryStream.Length);
//
//         await fileStorageService.UploadFileAsync(memoryStream, newFile.GuidIdentifier, cancellationToken);
//         
//         // link to file
//         
//         var retainCacheFor = TimeSpan.FromHours(48);
//         var memCacheFileSize = (int)Math.Round((double)newFile.Size / 1024 / 1024);
//         
//         using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
//         
//         try
//         {
//             context.PersonalFiles.Add(newFile);
//             await context.SaveChangesAsync(cancellationToken);
//         }
//         catch
//         {
//             backgroundJobService.Enqueue(
//                 () => fileStorageService.DeleteFileAsync(newFile.GuidIdentifier, CancellationToken.None),
//                 BackgroundJobQueues.PersonalFileStorage);
//             throw;
//         }
//         
//         backgroundJobService.Enqueue(
//             () => ProcessFileAsync(newFile, memCacheFileSize, retainCacheFor),
//             BackgroundJobQueues.PersonalFileTextExtractionAndSearch
//         );
//         
//         transaction.Complete();
//
//         try
//         {
//             memoryCache.Set(newFile.GuidIdentifier, memoryStream.ToArray(),
//                 new MemoryCacheEntryOptions().SetSize(memCacheFileSize).SetAbsoluteExpiration(retainCacheFor));
//         }
//         catch
//         {
//             // ignored
//         }
//
//         return Result.Success();
//     }
//
//
//     private async Task ProcessFileAsync(PersonalFile fileEntity, long memCacheFileSize, TimeSpan retainCacheFor)
//     {
//         memoryCache.TryGetValue(fileEntity.GuidIdentifier, out byte[]? fileBytes);
//
//         if (fileBytes is null)
//         {
//             await using var downloadStream = await fileStorageService
//                 .GetDownloadStreamAsync(fileEntity.GuidIdentifier, CancellationToken.None);
//
//             using var memoryStream = new MemoryStream();
//             await downloadStream.CopyToAsync(memoryStream);
//             fileBytes = memoryStream.ToArray();
//
//             memoryCache.Set(fileEntity.GuidIdentifier, fileBytes,
//                 new MemoryCacheEntryOptions().SetSize(memCacheFileSize).SetAbsoluteExpiration(retainCacheFor));
//         }
//
//         memoryCache.TryGetValue($"{fileEntity.GuidIdentifier.ToString()}_text", out string? text);
//
//         if (text is null)
//         {
//             text = await textExtractionService.ExtractTextAsync(fileBytes, fileEntity.Extension, CancellationToken.None);
//
//             memoryCache.Set($"{fileEntity.GuidIdentifier.ToString()}_text", text, 
//                 new MemoryCacheEntryOptions().SetSize(1).SetAbsoluteExpiration(retainCacheFor));
//         }
//
//         if (text != "")
//         {
//             await personalFileSearchRepository
//                 .UpdateAsync(new PersonalFileSearchModel(fileEntity.Id, text));
//         }
//
//         try
//         {
//             memoryCache.Remove(fileEntity.GuidIdentifier);
//         }
//         catch
//         {
//             // ignored
//         }
//
//         try
//         {
//             memoryCache.Remove($"{fileEntity.GuidIdentifier.ToString()}_text");
//         }
//         catch
//         {
//             // ignored
//         }
//     }
// }