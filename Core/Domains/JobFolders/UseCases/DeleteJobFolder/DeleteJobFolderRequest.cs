using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobFolders.UseCases.DeleteJobFolder;

public record DeleteJobFolderRequest(long Id) : IRequest<Result>;