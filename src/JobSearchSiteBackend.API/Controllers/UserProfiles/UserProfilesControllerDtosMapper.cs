using AutoMapper;
using JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.AddUserProfile;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetJobApplications;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetPersonalFiles;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetUserProfile;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.UploadUserAvatar;

namespace JobSearchSiteBackend.API.Controllers.UserProfiles;

public class UserProfilesControllerDtosMapper : Profile
{
    public UserProfilesControllerDtosMapper()
    {
        CreateMap<AddUserProfileResult, AddUserProfileResponse>();
        
        CreateMap<GetBookmarkedJobsResult, GetBookmarkedJobsResponse>();
        
        CreateMap<GetJobApplicationsResult, GetJobApplicationsResponse>();
        
        CreateMap<GetPersonalFilesResult, GetPersonalFilesResponse>();
        
        CreateMap<GetUserProfileResult, GetUserProfileResponse>();
        
        CreateMap<UploadUserAvatarResult, UploadUserAvatarResponse>();
    }
}