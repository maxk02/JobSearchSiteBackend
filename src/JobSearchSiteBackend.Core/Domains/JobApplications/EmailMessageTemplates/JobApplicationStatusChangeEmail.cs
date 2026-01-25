using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Core.Services.EmailSender;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.EmailMessageTemplates;

public class JobApplicationStatusChangeEmail(JobApplicationStatus newStatus, string jobName, string companyName) : IEmailTemplate
{
    public string Subject => $"Zmiana statusu aplikacji na stanowisko {jobName} ({companyName})";
    
    public string Content => newStatus switch
    {
         JobApplicationStatus.Rejected => $"""
                                          <p>
                                             Witamy!
                                             <br><br>
                                             Dziękujemy za zainteresowanie ogłoszeniem o pracę oraz udział w procesie rekrutacyjnym na stanowisko {jobName} w firmie {companyName}.
                                             <br><br>
                                             Po przeanalizowaniu zgłoszeń otrzymaliśmy informację od pracodawcy, że w rekrutacji podjęto decyzję o kontynuowaniu procesu z innymi kandydatami.
                                             <br><br>
                                             Zachęcamy do dalszego korzystania z naszej platformy i aplikowania na inne oferty, które mogą odpowiadać Państwa doświadczeniu oraz oczekiwaniom.
                                             <br><br>
                                             Życzymy powodzenia w kolejnych etapach poszukiwania pracy.
                                             <br><br>
                                             Z wyrazami szacunku,
                                             <br>
                                             Zespół znajdzprace.pl
                                          </p>
                                          """,
         JobApplicationStatus.Seen => $"""
                                       <p>
                                          Witamy!
                                          <br><br>
                                          Mamy dobrą wiadomość! Twoja aplikacja na stanowisko {jobName} w firmie {companyName} została właśnie wyświetlona przez rekrutera.
                                          <br><br>
                                          Oznacza to, że pracodawca zapoznaje się obecnie z Twoim profilem oraz przesłanymi dokumentami. 
                                          <br><br>
                                          Prosimy o cierpliwość – proces analizy kandydatur może chwilę potrwać. Jeśli Twoje zgłoszenie spotka się z zainteresowaniem, otrzymasz powiadomienie o zmianie statusu lub bezpośredni kontakt ze strony firmy.
                                          <br><br>
                                          Trzymamy kciuki!
                                          <br><br>
                                          Z wyrazami szacunku,
                                          <br>
                                          Zespół znajdzprace.pl
                                       </p>
                                       """,
         JobApplicationStatus.Shortlisted => $"""
                                             <p>
                                                Witamy, 
                                                <br><br>
                                                Dziękujemy za zgłoszenie na ofertę pracy oraz przesłanie swojej aplikacji na stanowisko {jobName}.
                                                <br><br>
                                                Otrzymaliśmy informację od ${companyName}, że Państwa kandydatura została dodana do puli osób rozważanych do przeprowadzenia rozmowy kwalifikacyjnej.
                                                <br><br>
                                                W najbliższym czasie pracodawca może skontaktować się z Panią/Panem bezpośrednio, aby przekazać dalsze informacje lub zaprosić do kolejnego etapu rekrutacji. Zachęcamy do regularnego sprawdzania swojej skrzynki odbiorczej oraz danych kontaktowych podanych w aplikacji.
                                                <br><br>
                                                Życzymy powodzenia w kolejnych etapach poszukiwania pracy.
                                                <br><br>
                                                Z wyrazami szacunku,
                                                <br>
                                                Zespół znajdzprace.pl
                                             </p>
                                             """,
        _ => throw new ArgumentException()
    };
}