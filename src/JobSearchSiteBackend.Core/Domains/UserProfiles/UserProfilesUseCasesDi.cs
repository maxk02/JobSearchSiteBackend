using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.AddJobBookmark;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.AddUserProfile;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.DeleteJobBookmark;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetJobApplications;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetPersonalFiles;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetUserProfile;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.UpdateUserProfile;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.UploadUserAvatar;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles;

public static class UserProfilesUseCasesDi
{
    public static void ConfigureUserProfileUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddJobBookmarkHandler>();
        serviceCollection.AddScoped<AddUserProfileHandler>();
        serviceCollection.AddScoped<DeleteJobBookmarkHandler>();
        serviceCollection.AddScoped<GetBookmarkedJobsHandler>();
        serviceCollection.AddScoped<GetJobApplicationsHandler>();
        serviceCollection.AddScoped<GetPersonalFilesHandler>();
        serviceCollection.AddScoped<GetUserProfileHandler>();
        serviceCollection.AddScoped<UpdateUserProfileHandler>();
        serviceCollection.AddScoped<UploadUserAvatarHandler>();
    }
}