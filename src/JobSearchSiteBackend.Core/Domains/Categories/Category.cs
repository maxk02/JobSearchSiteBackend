using System.Collections.Immutable;
using JobSearchSiteBackend.Core.Domains._Shared.EntityInterfaces;
using JobSearchSiteBackend.Core.Domains.Jobs;

namespace JobSearchSiteBackend.Core.Domains.Categories;

public class Category : IEntityWithId
{
    public static readonly ImmutableArray<Category> AllValues = [
        new Category(1, "Administracja biurowa"),
        new Category(2, "Administracja publiczna / Służba cywilna"),
        new Category(3, "Architektura"),
        new Category(4, "Badania i rozwój"),
        new Category(5, "Budownictwo / Geodezja"),
        new Category(6, "Doradztwo / Konsulting"),
        new Category(7, "Edukacja / Nauka / Szkolenia"),
        new Category(8, "Energetyka / Elektronika"),
        new Category(9, "Farmaceutyka / Biotechnologia"),
        new Category(10, "Finanse / Bankowość"),
        new Category(11, "Gastronomia / Catering"),
        new Category(12, "Grafika / Fotografia / Kreacja"),
        new Category(13, "Human Resources / Kadry"),
        new Category(14, "Informatyka / Administracja"),
        new Category(15, "Informatyka / Programowanie"),
        new Category(16, "Internet / e-commerce"),
        new Category(17, "Inżynieria / Projektowanie"),
        new Category(18, "Kadra zarządzająca"),
        new Category(19, "Kontrola jakości"),
        new Category(20, "Kosmetyka / Pielęgnacja"),
        new Category(21, "Księgowość / Audyt / Podatki"),
        new Category(22, "Logistyka / Dystrybucja"),
        new Category(23, "Marketing / Reklama / PR"),
        new Category(24, "Media / Sztuka / Rozrywka"),
        new Category(25, "Medycyna / Opieka zdrowotna"),
        new Category(26, "Motoryzacja"),
        new Category(27, "Nieruchomości"),
        new Category(28, "Ochrona osób i mienia"),
        new Category(29, "Organizacje pozarządowe / Wolontariat"),
        new Category(30, "Praca fizyczna"),
        new Category(31, "Praktyki / Staż"),
        new Category(32, "Prawo"),
        new Category(33, "Przemysł / Produkcja"),
        new Category(34, "Rolnictwo / Ochrona środowiska"),
        new Category(35, "Serwis / Technika / Montaż"),
        new Category(36, "Sport / Rekreacja"),
        new Category(37, "Sprzedaż / Obsługa klienta"),
        new Category(38, "Telekomunikacja"),
        new Category(39, "Tłumaczenia"),
        new Category(40, "Transport / Spedycja"),
        new Category(41, "Turystyka / Hotelarstwo"),
        new Category(42, "Ubezpieczenia"),
        new Category(43, "Zakupy"),
        new Category(44, "Franczyza")
    ];
    
    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(category => category.Id)];

    public Category(long id, string namePl)
    {
        Id = id;
        NamePl = namePl;
    }
    
    public long Id { get; }
    
    public string NamePl { get; private set; }
    
    public ICollection<Job>? Jobs { get; set; }
}