using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.JobFolders.UseCases.UpdateJobFolder;

public record UpdateJobFolderRequest(long Id, string? Name, string? Description) : IRequest<Result>;