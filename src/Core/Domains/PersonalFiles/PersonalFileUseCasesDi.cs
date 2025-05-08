using Core.Domains.PersonalFiles.UseCases.DeleteFile;
using Core.Domains.PersonalFiles.UseCases.UpdateFile;
using Core.Domains.PersonalFiles.UseCases.UploadFile;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domains.PersonalFiles;

public static class PersonalFileUseCasesDi
{
    public static void ConfigurePersonalFileUseCases(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<DeleteFileHandler>();
        serviceCollection.AddScoped<UpdateFileHandler>();
        serviceCollection.AddScoped<UploadFileHandler>();
    }
}