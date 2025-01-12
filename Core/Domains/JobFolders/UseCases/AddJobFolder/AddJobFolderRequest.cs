using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobFolders.UseCases.AddJobFolder;

public record AddJobFolderRequest(long CompanyId, long ParentId,
    string? Name, string? Description) : IRequest<Result<long>>;