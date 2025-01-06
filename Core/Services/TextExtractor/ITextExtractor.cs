namespace Core.Services.TextExtractor;

public interface ITextExtractor
{
    Task<string> ExtractTextAsync(Stream fileStream);
}