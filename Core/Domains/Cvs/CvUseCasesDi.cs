// using Core.Domains.Cvs.UseCases.AddCv;
// using Core.Domains.Cvs.UseCases.DeleteCv;
// using Core.Domains.Cvs.UseCases.GetCvById;
// using Core.Domains.Cvs.UseCases.UpdateCv;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace Core.Domains.Cvs;
//
// public static class CvUseCasesDi
// {
//     public static void ConfigureCvUseCases(this IServiceCollection serviceCollection)
//     {
//         serviceCollection.AddScoped<AddCvHandler>();
//         serviceCollection.AddScoped<DeleteCvHandler>();
//         serviceCollection.AddScoped<GetCvByIdHandler>();
//         serviceCollection.AddScoped<UpdateCvHandler>();
//     }
// }