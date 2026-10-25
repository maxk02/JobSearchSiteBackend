using JobSearchSiteBackend.Main.Domains.PersonalFiles.UseCases.DeleteFile;
using JobSearchSiteBackend.Main.Domains.PersonalFiles.UseCases.GetDownloadLink;
using JobSearchSiteBackend.Main.Domains.PersonalFiles.UseCases.UpdateFile;
using JobSearchSiteBackend.Main.Domains.PersonalFiles.UseCases.UploadFile;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Main.Domains.PersonalFiles;

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