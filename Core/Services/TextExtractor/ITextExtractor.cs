namespace Core.Services.TextExtractor;

public interface ITextExtractor
{
    Task<string> ExtractTextAsync(byte[] fileContent, string extension, CancellationToken cancellationToken = default);
}