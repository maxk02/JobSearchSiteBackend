using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.DeleteFile;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.GetDownloadLink;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UpdateFile;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UploadFile;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles;

public static class PersonalFileUseCasesDi
{
    public static void ConfigurePersonalFileUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<DeleteFileHandler>();
        serviceCollection.AddScoped<GetDownloadLinkHandler>();
        serviceCollection.AddScoped<UpdateFileHandler>();
        serviceCollection.AddScoped<UploadFileHandler>();
    }
}