using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.JobFolders.UseCases.AddJobFolder;

public record AddJobFolderRequest(long CompanyId, long ParentId,
    string? Name, string? Description) : IRequest<Result<long>>;