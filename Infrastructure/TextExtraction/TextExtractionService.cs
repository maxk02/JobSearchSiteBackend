using Core.Services.TextExtraction;
using TikaOnDotNet.TextExtraction;

namespace Infrastructure.TextExtraction;

public class TextExtractionService : ITextExtractionService
{
    public async Task<string> ExtractTextAsync(byte[] fileContent, string extension,
        CancellationToken cancellationToken = default)
    {
        var text = "";
        
        await Task.Run(() =>
        {
            var textExtractor = new TextExtractor();
            var result = textExtractor.Extract(fileContent);
            text = result.Text;
        }, cancellationToken);
        
        return text;
    }
}