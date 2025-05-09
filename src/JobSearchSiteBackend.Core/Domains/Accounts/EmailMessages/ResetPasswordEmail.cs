using JobSearchSiteBackend.Core.Domains._Shared.Email;

namespace JobSearchSiteBackend.Core.Domains.Accounts.EmailMessages;

public class ResetPasswordEmail(string resetLink) : IEmailModel
{
    public string Subject => "Resetowanie hasła";

    public string Content =>
        $"Witamy, przed chwilą otrzymaliśmy żądanie resetowania hasła w serwisie znadjzprace.pl." +
        $" Prosimy o kliknięcie w link poniżej w celu resetowania hasła lub o" +
        $" ignorowanie tej wiadomości w razie niewysyłania tego żądania." +
        $"<br><br>Link: <a href='{resetLink}'>{resetLink}</a>";
}