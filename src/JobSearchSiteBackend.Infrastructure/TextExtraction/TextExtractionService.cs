using System.Text;
using JobSearchSiteBackend.Core.Services.TextExtraction;
using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Pdf.IO;

namespace JobSearchSiteBackend.Infrastructure.TextExtraction;

public class TextExtractionService : ITextExtractionService
{
    public async Task<string> ExtractTextAsync(Stream fileStream, string extension, CancellationToken cancellationToken = default)
    {
        if (fileStream.Length == 0)
            throw new ArgumentException("File content cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("File extension cannot be null or empty.", nameof(extension));
        
        extension = extension.ToLowerInvariant();

        return extension switch
        {
            ".docx" => await ExtractTextFromDocxAsync(fileStream, cancellationToken),
            ".pdf" => ExtractTextFromPdf(fileStream),
            _ => throw new NotSupportedException($"The file format '{extension}' is not supported.")
        };
    }

    private static async Task<string> ExtractTextFromDocxAsync(Stream fileStream, CancellationToken cancellationToken)
    {
        fileStream.Position = 0;

        using var wordDoc = WordprocessingDocument.Open(fileStream, false);
        var stringBuilder = new StringBuilder();

        foreach (var element in wordDoc.MainDocumentPart?.Document.Body?.Elements() ?? [])
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            stringBuilder.AppendLine(element.InnerText);
        }

        return await Task.FromResult(stringBuilder.ToString());
    }

    private static string ExtractTextFromPdf(Stream fileStream)
    {
        fileStream.Position = 0;

        using var pdfDoc = PdfReader.Open(fileStream, PdfDocumentOpenMode.Import);
        var stringBuilder = new StringBuilder();

        foreach (var page in pdfDoc.Pages)
        {
            var content = page.Contents.CreateSingleContent();
            stringBuilder.AppendLine(content.ToString());
        }

        return stringBuilder.ToString();
    }
}