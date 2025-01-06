namespace Core.Services.TextExtractor;

public interface ITextExtractor
{
    Task<string?> ExtractTextAsync(Stream fileStream, string extension, CancellationToken cancellationToken = default);
}