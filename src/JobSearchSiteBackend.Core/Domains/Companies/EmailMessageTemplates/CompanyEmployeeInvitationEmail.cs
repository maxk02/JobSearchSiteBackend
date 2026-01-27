using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Core.Domains.Companies.EmailMessageTemplates;

public class CompanyEmployeeInvitationEmail(string ConfirmationLink, string CompanyName) : IEmailTemplate
{
    public string Subject => 
        $"Zaproszenie do zarządzania firmą {CompanyName}";
    
    public string Content =>
        $"""
         <p>
            Witamy,
            <br><br>
            na Państwa konto wpłynęło zaproszenie z firmy {CompanyName}.
            <br><br>
            W razie akceptacji zaproszenia, administratorzy firmy będą mogli wyszukiwać to konto wśród kont pracowników firmy i zarządzać jego uprawnieniami.
            <br><br>
            W celu akceptacji prosimy prejść na stronę: <a href='{ConfirmationLink}'>{ConfirmationLink}</a>.
            <br><br>
            Z wyrazami szacunku,
            <br>
            Zespół znajdzprace.pl
         </p>
         """;
}