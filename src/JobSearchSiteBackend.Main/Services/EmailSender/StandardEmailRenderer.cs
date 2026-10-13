using JobSearchSiteBackend.Core.Services.FileStorage;

namespace JobSearchSiteBackend.Core.Services.EmailSender;

public class StandardEmailRenderer()
{
    public async Task<RenderedEmailTemplate> RenderAsync(IEmailTemplate template)
    {
        var renderedBody = $"""
                            <svg width="260" height="60" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 300 60">
                                <rect width="260" height="60" fill="transparent" />

                                <text x="55" y="38.5" font-family="Arial" font-size="28" font-weight="bold" fill="#202557">znajdz</text>
                                <text x="140" y="38.5" font-family="Arial" font-size="28" font-weight="bold" fill="#4A90E2">prace</text>
                                <text x="213" y="38.5" font-family="Arial" font-size="28" font-weight="bold" fill="#202557">.pl</text>

                                <circle cx="25" cy="28" r="15" stroke="#4A90E2" stroke-width="3" fill="none"/>
                                <line x1="35" y1="39" x2="45" y2="50" stroke="#4A90E2" stroke-width="3" stroke-linecap="round"/>
                            </svg>
                            <br>
                            {template.Content}
                            <p>&copy;2026 znajdzprace.pl Wszystkie prawa zastrzeżone.</p>
                            """;
        
        return new RenderedEmailTemplate(template.Subject, renderedBody, true);
    }
}