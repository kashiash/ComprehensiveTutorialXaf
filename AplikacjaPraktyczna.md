# Aplikacja bardziej praktyczna

Nie będe sie skupiał nad tym jak zaczac projekt, bo to jest w oryginalnym tutorialu.

Tutaj definiujemy dodatkowe klasy BO, wypełniamy je danymi, robimy wydruki itd
## Klasy BO
Zaczynamy od definicji potrzebnych klas.
### Kody pocztowe, gminy miast województwa

Zdefiniujemy tabele na województwa, powiaty gminy i miasta z kodami pocztowymi. Dodatkowo przedstawię kod jak takie dane zaimportować

[Kody pocztowe](KodyPocztowe.md) - Kody pocztowe i ich import z pliku dostarczanego przez PP

### Kontrahent , adresy, pracownicy

Dane klientów, kontaktór, spotkań
[Definicje klas](Klienci.md)
### Faktura, Towary, Wpłaty

[Definicje klas](FakturyWplatyTowary.md)
### Magazyny
TBD



### Wygenerowanie danych

#### Kody pocztowe importujemy zgodnie z opisem :
[Kody pocztowe](KodyPocztowe.md)

Kraje i waluty mamy w systemie wystarczy prostym kodem je wyciagnąć:
```csharp
void DodajKraje(IObjectSpace os)
   {

       foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.FrameworkCultures))

       {
           RegionInfo ri = null;
           try
           {
               ri = new RegionInfo(ci.Name);
           }
           catch
           {

               continue;
           }
           var a = ri.EnglishName;
           var a1 = ri.NativeName;
           var a2 = ri.ThreeLetterISORegionName;
           var a3 = ri.CurrencyEnglishName;
           var a4 = ri.CurrencyNativeName;
           var a5 = ri.CurrencySymbol;
           var a6 = ri.ISOCurrencySymbol;

           var waluta = ObjectSpace.FindObject<Waluta>(new BinaryOperator("Symbol", ri.ISOCurrencySymbol));
           if (waluta == null)
           {
               waluta = ObjectSpace.CreateObject<Waluta>();
               waluta.Symbol = ri.ISOCurrencySymbol;
               waluta.Nazwa = ri.CurrencyEnglishName;
               waluta.LokalnaNazwa = ri.CurrencyNativeName;
               waluta.LokalnySymbol = ri.CurrencySymbol;
           }

           var kraj = ObjectSpace.FindObject<Kraj>(new BinaryOperator("Symbol", ri.ThreeLetterISORegionName));
           if (kraj == null)
           {
               kraj = ObjectSpace.CreateObject<Kraj>();
               kraj.Symbol = ri.ThreeLetterISORegionName;
               kraj.Nazwa = ri.EnglishName;
               kraj.LokalnySymbol = ri.TwoLetterISORegionName;
               kraj.LokalnaNazwa = ri.NativeName;
               kraj.GeoId = ri.GeoId;
               kraj.Waluta = waluta;
               kraj.IsMetric = ri.IsMetric;

           }
           waluta.Kraj = kraj;
       }
   }
```

Dane klientów, wygenerujemy używając biblioteki o nazwie Bogus:





```csharp
 private void PrzygotujDaneTestowe()
        {
            var stawki = new List<StawkaVAT>();
            stawki.Add(NowaStawka("23%", 23M));
            stawki.Add(NowaStawka("0%", 0M));
            stawki.Add(NowaStawka("7%", 7M));
            stawki.Add(NowaStawka("ZW", 0M));


            var stawki = ObjectSpace.GetObjectsQuery<StawkaVAT>().ToList();

            var cusFaker = new Faker<Klient>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Klient>())
            .RuleFor(o => o.Telefon, f => f.Person.Phone)
            .RuleFor(o => o.Skrot, f => f.Company.CompanyName())
            .RuleFor(o => o.Nazwa, f => f.Company.CompanyName())
            .RuleFor(o => o.Email, (f, u) => f.Internet.Email())
            .RuleFor(o => o.Miejscowosc, f => f.Address.City())
            .RuleFor(o => o.KodPocztowy, f => f.Address.ZipCode())
            .RuleFor(o => o.Ulica, f => f.Address.StreetName());
            var customers = cusFaker.Generate(10000);
            ObjectSpace.CommitChanges();

            var conFaker = new Faker<Kontakt>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Kontakt>())
            .RuleFor(o => o.Imie, f => f.Person.FirstName)
            .RuleFor(o => o.Nazwisko, f => f.Person.LastName)
            .RuleFor(o => o.Nazwisko, f => f.Person.LastName)
            .RuleFor(o => o.Email, (f, u) => f.Internet.Email())
            .RuleFor(o => o.Telefon, f => f.Person.Phone);

            var contacts = conFaker.Generate(10000);
            ObjectSpace.CommitChanges();



            var meetFaker = new Faker<Spotkanie>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Spotkanie>())
            .RuleFor(o => o.Klient, f => f.PickRandom(customers))
            .RuleFor(o => o.StartOn, f => f.Date.Soon(10));
            var meetings = meetFaker.Generate(1000);


            //  var stawki = new[] { "23%", "7%", "ZW", "0%" };
            var prodFaker = new Faker<Produkt>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Produkt>())
            .RuleFor(o => o.Nazwa, f => f.Commerce.ProductName())
            .RuleFor(o => o.StawkaVAT, f => f.PickRandom(stawki))
            .RuleFor(o => o.Cena, f => f.Random.Decimal(0.01M, 1000M));

            var products = prodFaker.Generate(100);

            WygenerujFaktury(1000, customers, products);
        }

        private StawkaVAT NowaStawka(string symbol, decimal wartosc)
        {
            var stawka = ObjectSpace.CreateObject<StawkaVAT>();
            stawka.Symbol = symbol;
            stawka.Stawka = wartosc;
            return stawka;
        }

private void WygenerujFaktury(int liczbaFaktur, IList<Klient> customers, IList<Produkt> products)
{
    if (customers is null)
    {
        customers = ObjectSpace.GetObjectsQuery<Klient>().ToList();
    }

    var orderFaker = new Faker<Faktura>("pl")
    .CustomInstantiator(f => ObjectSpace.CreateObject<Faktura>())
    .RuleFor(o => o.NumerFaktury, f => f.Random.Int().ToString())
    .RuleFor(o => o.DataFaktury, f => f.Date.Past(2))
    .RuleFor(o => o.DataSprzedazy, f => f.Date.Past(20))
    .RuleFor(o => o.Klient, f => f.PickRandom(customers));
    var orders = orderFaker.Generate(liczbaFaktur);
    if (products == null)
    {
        products = ObjectSpace.GetObjectsQuery<Produkt>().ToList();
    }

    var itemsFaker = new Faker<PozycjaFaktury>()
    .CustomInstantiator(f => ObjectSpace.CreateObject<PozycjaFaktury>())
    .RuleFor(o => o.Faktura, f => f.PickRandom(orders))
    .RuleFor(o => o.Produkt, f => f.PickRandom(products))

    .RuleFor(o => o.Ilosc, f => f.Random.Decimal(0.01M, 100M));

    var items = itemsFaker.Generate(liczbaFaktur * 10);
}
```
