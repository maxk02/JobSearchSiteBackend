using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.JobFolders.UseCases.DeleteJobFolder;

public record DeleteJobFolderRequest(long Id) : IRequest<Result>;