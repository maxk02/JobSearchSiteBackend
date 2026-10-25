using JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.AddJobBookmark;
using JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.AddUserProfile;
using JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.DeleteJobBookmark;
using JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.GetBookmarkedJobs;
using JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.GetJobApplications;
using JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.GetPersonalFiles;
using JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.GetUserProfile;
using JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.UpdateUserProfile;
using JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.UploadUserAvatar;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles;

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