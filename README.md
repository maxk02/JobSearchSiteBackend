# JobSearchSiteBackend

## Informacje ogólne
To repozytorium zawiera przykładowe API dla strony do poszukiwania pracy.

Aplikację przedstawiono jako rozwiązanie C#/.NET 9 zawierające trzy projekty (Core, Infrastructure, API) reprezentujące uproszczoną interpretację Clean Architecture oraz projekt Shared, w którym umieszczono wspólne dla całego rozwiązania klasy konfiguracji oraz rozszerzeń.


## Baza danych
![Schemat bazy danych](https://raw.githubusercontent.com/maxk02/JobSearchSiteBackend/main/assets/mainDbSchema.png)


## Projekt Core
Core jest projektem rdzennym aplikacji z zaimplementowanymi encjami, logiką biznesową oraz interfejsami usług zewnętrznych.

Unika zależności od frameworku webowego oraz paczek zewnętrznych z zauważalnymi wyjątkami w postaci EF Core występującego jako stosunkowo abstrakcyjne repozytorium do interakcji z bazą danych, FluentValidation ułatwiającego walidację encji i reguł oraz AutoMapper wspomagającego mapowanie obiektów DTO.

Większość projektu składa się z:
- folderów domen gromadzących definicje encji wraz z przypadkami użycia,
- interfejsów (kontraktów) usług zewnętrznych do implementacji w warstwach zewnętrznych.


## Projekt Infrastructure
Infrastructure jest projektem odpowiedzialnym za implementację interfejsów zdefiniowanych w projekcie Core oraz za konfigurację i rejestrację usług zewnętrznych.


## Projekt API
API jest projektem odpowiedzialnym za komunikację z klientem. Wykorzystuje ASP.NET Core do obsługi żądań HTTP i zwracania odpowiedzi w formacie JSON.

Zawiera kontrolery, które są odpowiedzialne za przyjmowanie żądań, wstępną walidację danych wejściowych oraz wywoływanie odpowiednich przypadków użycia z projektu Core.


## Projekt Shared
Shared jest projektem zawierającym klasy pomocnicze, które są współdzielone pomiędzy różnymi projektami w rozwiązaniu. Zawiera klasy wspólne dla całego rozwiązania, takie jak klasy konfiguracji, rozszerzenia oraz inne elementy, które mogą być używane we wszystkich projektach.
