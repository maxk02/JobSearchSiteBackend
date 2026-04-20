using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Core.Domains.Accounts.EmailMessageTemplates;

public class ResetPasswordEmail(string resetLink) : IEmailTemplate
{
    public string Subject => "Resetowanie hasła";
    
    public string Content =>
        $"""
         <p>
            Witamy,
            <br><br>
            przed chwilą otrzymaliśmy żądanie resetowania hasła w serwisie znadjzprace.pl.
            <br><br>
            Prosimy o kliknięcie w link poniżej w celu resetowania hasła lub o ignorowanie tej wiadomości w razie niewysyłania takiego żądania.
            <br><br>
            <a href='{resetLink}'>Link do resetowania hasła</a>
            <br><br>
            Z wyrazami szacunku,
            <br>
            Zespół znajdzprace.pl
         </p>
         """;
}