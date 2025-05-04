using Core.Domains.UserProfiles.UseCases.AddJobBookmark;
using Core.Domains.UserProfiles.UseCases.AddUserProfile;
using Core.Domains.UserProfiles.UseCases.DeleteJobBookmark;
using Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;
using Core.Domains.UserProfiles.UseCases.GetJobApplications;
using Core.Domains.UserProfiles.UseCases.GetPersonalFiles;
using Core.Domains.UserProfiles.UseCases.GetUserProfile;
using Core.Domains.UserProfiles.UseCases.UpdateUserProfile;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domains.UserProfiles;

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
    }
}