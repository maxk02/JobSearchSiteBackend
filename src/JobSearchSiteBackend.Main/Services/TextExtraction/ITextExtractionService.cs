namespace JobSearchSiteBackend.Main.Services.TextExtraction;

public interface ITextExtractionService
{
    Task<string> ExtractTextAsync(Stream fileStream, string extension, CancellationToken cancellationToken = default);
}