using Core.Domains.UserProfiles.UseCases.AddCompanyBookmark;
using Core.Domains.UserProfiles.UseCases.AddJobBookmark;
using Core.Domains.UserProfiles.UseCases.AddUserProfile;
using Core.Domains.UserProfiles.UseCases.DeleteCompanyBookmark;
using Core.Domains.UserProfiles.UseCases.DeleteJobBookmark;
using Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;
using Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;
using Core.Domains.UserProfiles.UseCases.GetFirstCv;
using Core.Domains.UserProfiles.UseCases.GetJobApplicationsForUser;
using Core.Domains.UserProfiles.UseCases.GetPersonalFiles;
using Core.Domains.UserProfiles.UseCases.GetUserProfileById;
using Core.Domains.UserProfiles.UseCases.UpdateUserProfile;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domains.UserProfiles;

public static class UserProfilesUseCasesDi
{
    public static void ConfigureUserProfileUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddCompanyBookmarkHandler>();
        serviceCollection.AddScoped<AddJobBookmarkHandler>();
        serviceCollection.AddScoped<AddUserProfileHandler>();
        serviceCollection.AddScoped<DeleteCompanyBookmarkHandler>();
        serviceCollection.AddScoped<DeleteJobBookmarkHandler>();
        serviceCollection.AddScoped<GetBookmarkedCompaniesHandler>();
        serviceCollection.AddScoped<GetBookmarkedJobsHandler>();
        serviceCollection.AddScoped<GetFirstCvHandler>();
        serviceCollection.AddScoped<GetJobApplicationsForUserHandler>();
        serviceCollection.AddScoped<GetPersonalFilesHandler>();
        serviceCollection.AddScoped<GetUserProfileByIdHandler>();
        serviceCollection.AddScoped<UpdateUserProfileHandler>();
    }
}