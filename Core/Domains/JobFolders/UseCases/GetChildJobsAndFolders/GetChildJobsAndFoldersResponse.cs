using Core.Domains.JobFolders.Dtos;
using Core.Domains.Jobs.Dtos;

namespace Core.Domains.JobFolders.UseCases.GetChildJobsAndFolders;

public record GetChildJobsAndFoldersResponse(ICollection<JobInfocardInFolderDto> Jobs,
    ICollection<JobFolderDto> Folders);