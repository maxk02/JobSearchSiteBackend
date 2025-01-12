using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobFolders.UseCases.UpdateJobFolder;

public record UpdateJobFolderRequest(long Id, string? Name, string? Description) : IRequest<Result>;