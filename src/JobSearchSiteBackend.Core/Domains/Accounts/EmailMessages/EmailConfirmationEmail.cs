using JobSearchSiteBackend.Core.Domains._Shared.Email;

namespace JobSearchSiteBackend.Core.Domains.Accounts.EmailMessages;

public class EmailConfirmationEmail(string confirmationLink) : IEmailModel
{
    public string Subject => 
        "Aktywacja konta";

    public string Content =>
        $"Witamy, dziękujemy za rejestrację w serwisie znadjzprace.pl." +
        $" Prosimy o podtwierdzenie adresu e-mail poprzez kliknięcie w link poniżej w celu" +
        $" aktywacji konta:<br><a href='{confirmationLink}'>link</a>";
}