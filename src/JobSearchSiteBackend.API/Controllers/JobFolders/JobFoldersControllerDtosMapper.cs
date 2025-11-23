using AutoMapper;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.UpdateJobFolder;
using UpdateJobFolderRequest = JobSearchSiteBackend.API.Controllers.JobFolders.Dtos.UpdateJobFolderRequest;

namespace JobSearchSiteBackend.API.Controllers.JobFolders;

public class JobFoldersControllerDtosMapper : Profile
{
    public JobFoldersControllerDtosMapper()
    {
        CreateMap<UpdateJobFolderRequest, UpdateJobFolderCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom((_, _, _, context) =>
            {
                if (context.Items.TryGetValue("Id", out var id))
                {
                    return (long)id;
                }

                return 0;
            }));
    }
}