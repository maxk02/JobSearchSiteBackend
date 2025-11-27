using AutoMapper;
using JobSearchSiteBackend.API.Controllers.PersonalFiles.Dtos;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.GetDownloadLink;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UpdateFile;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UploadFile;

namespace JobSearchSiteBackend.API.Controllers.PersonalFiles;

public class PersonalFilesControllerDtosMapper : Profile
{
    public PersonalFilesControllerDtosMapper()
    {
        CreateMap<GetDownloadLinkResult, GetDownloadLinkResponse>();
        
        CreateMap<UploadFileResult, UploadFileResponse>();
    }
}