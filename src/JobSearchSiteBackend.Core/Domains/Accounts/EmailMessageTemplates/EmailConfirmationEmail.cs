using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Core.Domains.Accounts.EmailMessageTemplates;

public class EmailConfirmationEmail(string confirmationLink) : IEmailTemplate
{
    public string Subject => 
        "Aktywacja konta";
    
    public string Content =>
        $"""
         <p>
            Witamy, dziękujemy za rejestrację w serwisie znadjzprace.pl.
            <br><br>
            Prosimy o podtwierdzenie adresu e-mail poprzez kliknięcie w link poniżej w celu aktywacji konta:
            <br>
            <a href='{confirmationLink}'>{confirmationLink}</a>
            <br><br>
            Z wyrazami szacunku,
            <br>
            Zespół znajdzprace.pl
         </p>
         """;
}