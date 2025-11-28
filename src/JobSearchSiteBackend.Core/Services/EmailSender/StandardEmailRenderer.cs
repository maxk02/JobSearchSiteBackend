using JobSearchSiteBackend.Core.Services.FileStorage;

namespace JobSearchSiteBackend.Core.Services.EmailSender;

public class StandardEmailRenderer(IFileStorageService fileStorageService)
{
    public async Task<RenderedEmailTemplate> RenderAsync(IEmailTemplate template)
    {
        var (guid, extension) = PublicAssetGuidsExtensions.EmailHeaderLogo;

        var logoUrl = await fileStorageService.GetPublicAssetUrlAsync(guid, extension);

        var renderedBody = $"""
                            <img src=""{logoUrl}"" />
                            {template.Content}
                            <p>&copy;2025 znajdzprace.pl Wszystkie prawa zastrzeżone.</p>
                            """;
        
        return new RenderedEmailTemplate(template.Subject, renderedBody, true);
    }
}