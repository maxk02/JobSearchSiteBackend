using Core.Domains.JobFolders.Dtos;
using Core.Domains.Jobs.Dtos;

namespace Core.Domains.JobFolders.UseCases.GetChildFolders;

public record GetChildFoldersResponse(ICollection<JobFolderDto> Folders);