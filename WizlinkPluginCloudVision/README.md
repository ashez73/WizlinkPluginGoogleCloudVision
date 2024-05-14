# Wizlink Plugin Sample

## Podstawy działania
- Aktywność Plugin potrafi wykonać publiczne metody bibliotek napisanych w .NET Framework (zalecana wersja 4.7.2).
- Wspierane formaty wyjściowe to WLL i DLL. Zalecanym formatem jest WLL, ponieważ pomaga to odróżnić plugin od jego zależności, które często są innymi plikami DLL. (W wersji Wizlinka 2.x rozszerzenie .wll jest nie tyle zalecane co wymagane).
- Po zbudowaniu pluginu należy skopiować go wraz z jego zależnościami do katalogu Plugins:
    ` %programdata%\WizLink\Plugins`
Dla wersji Wizlinka 2.*
    ` %programdata%\WizLink2\Plugins\FolderDlaDanegoPlugina`

## Dobre praktyki
### CancellationToken jako parametr wejściowy metod asynchronicznych 
- Aby wywołać metody asynchroniczne, zaleca się stosowanie parametru typu CancellationToken jako argumentu wejściowego. 
- Parametr ten nie jest widoczny w aktywności Plugin, ale jest automatycznie wykorzystywany przez mechanizm aktywności, gdy użytkownik przerywa scenariusz. 
- Metody Rest oraz DoAsync stanowią przykłady właściwego zastosowania tego mechanizmu.

### Dziedziczenie po PluginBase
- Klasa bazowa PluginBase zapewnia możliwość natychmiastowego przesyłania logów do kontekstu Wizlink.
- Atrybut Hide zapewnia możliwość ukrycia klasy lub metody w aktywności Plugin.
- Użycie atrybutu WizlinkVisible ogranicza widoczność klas w aktywności Plugin wyłącznie do tych, które zostały nim oznaczone.
- Atrybut TupleDescription pozwala na dodanie opisu poszczególnych elementów Tuple zwracanego z metody podobnie jak atrybut Description pozwala na opisanie całej metody lub jej poszczególnych parametrów

### Uwagi
- Projekt tworzy bibliotekę o rozszerzeniu DLL. Należy zmienić rozszerzenie na WLL przed skopiowaniem do folderu z pluginami Wizlinka.
- Scenariusz z przykładowym użyciem metod dostępnych w projekcie jest dołączony do projektu.
- Scenariusz Wizlinka, znajdujący się w folderze Scenario, działa z pluginem o nazwie WizlinkPlugin.WLL.
- Projekt o nazwie WizlinkPluginConsole to aplikacja konsolowa służąca do testowania pluginu.