using AutoMapper;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UpdateFile;

namespace JobSearchSiteBackend.API.Controllers.PersonalFiles;

public class PersonalFilesControllerDtosMapper : Profile
{
    public PersonalFilesControllerDtosMapper()
    {
        CreateMap<UpdateFileRequestDto, UpdateFileRequest>()
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