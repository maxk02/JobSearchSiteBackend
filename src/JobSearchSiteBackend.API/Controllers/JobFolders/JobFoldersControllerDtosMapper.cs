using AutoMapper;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.UpdateJobFolder;

namespace JobSearchSiteBackend.API.Controllers.JobFolders;

public class JobFoldersControllerDtosMapper : Profile
{
    public JobFoldersControllerDtosMapper()
    {
        CreateMap<UpdateJobFolderRequestDto, UpdateJobFolderRequest>()
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