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
                                             Witamy, 
                                             <br><br>
                                             Dziękujemy za zainteresowanie ogłoszeniem o pracę oraz udział w procesie rekrutacyjnym na stanowisko {jobName} w firmie {companyName}.
                                             <br>
                                             Po przeanalizowaniu zgłoszeń otrzymaliśmy informację od pracodawcy, że w rekrutacji podjęto decyzję o kontynuowaniu procesu z innymi kandydatami.
                                             <br>
                                             Zachęcamy do dalszego korzystania z naszej platformy i aplikowania na inne oferty, które mogą odpowiadać Państwa doświadczeniu oraz oczekiwaniom.
                                             <br>
                                             Życzymy powodzenia w kolejnych etapach poszukiwania pracy.
                                             <br>
                                             Z wyrazami szacunku,
                                             Zespół znajdzprace.pl
                                          </p>
                                          """,
        JobApplicationStatus.Shortlisted => $"""
                                             <p>
                                                Witamy, 
                                                <br><br>
                                                Dziękujemy za zgłoszenie na ofertę pracy oraz przesłanie swojej aplikacji na stanowisko {jobName}.
                                                <br>
                                                Otrzymaliśmy informację od ${companyName}, że Państwa kandydatura została dodana do puli osób rozważanych do przeprowadzenia rozmowy kwalifikacyjnej.
                                                <br>
                                                W najbliższym czasie pracodawca może skontaktować się z Panią/Panem bezpośrednio, aby przekazać dalsze informacje lub zaprosić do kolejnego etapu rekrutacji. Zachęcamy do regularnego sprawdzania swojej skrzynki odbiorczej oraz danych kontaktowych podanych w aplikacji.
                                                <br>
                                                Życzymy powodzenia w kolejnych etapach poszukiwania pracy.
                                                <br>
                                                Z wyrazami szacunku,
                                                Zespół znajdzprace.pl
                                             </p>
                                             """,
        JobApplicationStatus.OfferSent => $"""
                                           <p>
                                              Witamy, 
                                              <br><br>
                                              Gratulujemy! Otrzymaliśmy informację od ${companyName}, że w ramach procesu rekrutacyjnego na stanowisko {jobName} została dla Pani/Pana przygotowana oferta zatrudnienia.
                                              <br>
                                              Wkrótce otrzymają Państwo od pracodawcy szczegółowe informacje dotyczące warunków współpracy oraz dalszych kroków.
                                              <br>
                                              Życzymy powodzenia w finalizacji procesu oraz dalszych sukcesów zawodowych.
                                              <br>
                                              Z wyrazami szacunku,
                                              Zespół znajdzprace.pl
                                           </p>
                                           """,
        _ => throw new ArgumentException()
    };
}