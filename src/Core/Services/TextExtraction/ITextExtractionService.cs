namespace Core.Services.TextExtraction;

public interface ITextExtractionService
{
    Task<string> ExtractTextAsync(byte[] fileContent, string extension, CancellationToken cancellationToken = default);
}