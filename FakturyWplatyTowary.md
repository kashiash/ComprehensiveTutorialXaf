# Faktury, pozycje i wpłaty


## Faktura
```csharp
[ImageName("BO_Invoice")]
 [DefaultClassOptions]
 [XafDefaultProperty(nameof(NumerFaktury))]

 // kolorowanie rekordów
 [Appearance("FakturyZatwierdzone", Criteria = "Status = ##Enum#Demo1.Module.BusinessObjects.StatusFaktury,Zatwierdzona#", TargetItems = "*", FontColor = "Blue")]
 [Appearance("FakturyAnulowane", Criteria = "Status = ##Enum#Demo1.Module.BusinessObjects.StatusFaktury,Anulowana#", TargetItems = "*", FontColor = "Gray")]
 public class Faktura : XPObject
 {
     public Faktura(Session session) : base(session)
     { }



     StatusFaktury status;
     string numerFaktry;
     Klient klient;
     DateTime dataPlatnosci;
     DateTime dataSprzedazy;
     DateTime dataFaktury;



     [Size(SizeAttribute.DefaultStringMappingFieldSize)]
     [RuleRequiredField]
     [RuleUniqueValue]
     public string NumerFaktury

     {
         get => numerFaktry;
         set => SetPropertyValue(nameof(NumerFaktury), ref numerFaktry, value);
     }
     public DateTime DataFaktury
     {
         get => dataFaktury;
         set
         {

             var modified = SetPropertyValue(nameof(DataFaktury), ref dataFaktury, value);

             if (modified && !IsLoading && !IsSaving && Klient != null)
             {
                 DataPlatnosci = DataFaktury.AddDays(Klient.TerminPlatnosci);
             }
         }
     }

     public DateTime DataSprzedazy
     {
         get => dataSprzedazy;
         set => SetPropertyValue(nameof(DataSprzedazy), ref dataSprzedazy, value);
     }

     public DateTime DataPlatnosci
     {
         get => dataPlatnosci;
         set => SetPropertyValue(nameof(DataPlatnosci), ref dataPlatnosci, value);
     }


     [Association]

     public Klient Klient
     {
         get => klient;
         set
         {
             var modified = SetPropertyValue(nameof(Klient), ref klient, value);

             if (modified && !IsLoading && !IsSaving && Klient != null)
             {
                 DataPlatnosci = DataFaktury.AddDays(Klient.TerminPlatnosci);
             }
         }
     }


     [Persistent(nameof(WartoscNetto))]
     decimal wartoscNetto;
     [PersistentAlias(nameof(wartoscNetto))]
     public decimal WartoscNetto
     {
         get => wartoscNetto;
         set => SetPropertyValue(nameof(WartoscNetto), ref wartoscNetto, value);
     }
     [Persistent("WartoscVAT")]
     decimal? wartoscVAT;
     [PersistentAlias(nameof(wartoscVAT))]
     public decimal? WartoscVAT
     {
         get => wartoscVAT;
         set => SetPropertyValue(nameof(WartoscVAT), ref wartoscVAT, value);
     }
     [Persistent("WartoscBrutto")]
     decimal? wartoscBrutto;
     [PersistentAlias(nameof(wartoscBrutto))]
     public decimal? WartoscBrutto
     {
         get => wartoscBrutto;
         set => SetPropertyValue(nameof(WartoscBrutto), ref wartoscBrutto, value);
     }

     [Association, DevExpress.Xpo.Aggregated]
     public XPCollection<PozycjaFaktury> PozycjeFaktury
     {
         get
         {
             return GetCollection<PozycjaFaktury>(nameof(PozycjeFaktury));
         }
     }

     public override void AfterConstruction()
     {
         base.AfterConstruction();

         // ustawiamy wartości poczatkowe
         // Opowiednik w clarionie "On prime records"
         DataFaktury = DateTime.Now;
         Status = StatusFaktury.Przygotowana;

     }

     List<PodsumowanieVat> podsumowanieVat;
     [Delayed]
     public List<PodsumowanieVat> ListaVat {
         get {
             if (podsumowanieVat is null)
             {
                 PrzygotujListePodsumowujaca();
             }
             return podsumowanieVat;
         }
     }

     void PrzygotujListePodsumowujaca()
     {

         var pozycjeVat = from pz in PozycjeFaktury
                          group pz by pz.Produkt.StawkaVAT into PodsumowanieVat
                                select new PodsumowanieVat()
                                {
                                    StawkaVat = PodsumowanieVat.Key,
                                    Netto = PodsumowanieVat.Sum(s => s.WartoscNetto),
                                    Vat = PodsumowanieVat.Sum(s => s.WartoscVAT),
                                    Brutto = PodsumowanieVat.Sum(s => s.WartoscBrutto),
                                  //  Pozycje = PodsumowanieVat.ToList()
                                };
         podsumowanieVat = pozycjeVat.ToList();
         
     }

     public void PrzeliczSumy(bool forceChangeEvents)
     {
         decimal oldWartoscNetto = wartoscNetto;
         decimal? oldWartoscVAT = wartoscVAT;
         decimal? oldWartoscBrutto = wartoscBrutto;


         decimal tmpWartoscNetto = 0m;
         decimal? tmpWartoscVAT = 0m;
         decimal? tmpWartoscBrutto = 0m;

         foreach (PozycjaFaktury rec in PozycjeFaktury)
         {
             tmpWartoscNetto += rec.WartoscNetto;
             tmpWartoscVAT += rec.WartoscVAT;
             tmpWartoscBrutto += rec.WartoscBrutto;
         }
         wartoscNetto = tmpWartoscNetto;
         wartoscVAT = tmpWartoscVAT;
         wartoscBrutto = tmpWartoscBrutto;

         if (forceChangeEvents)
         {
             OnChanged(nameof(WartoscNetto), oldWartoscNetto, wartoscNetto);
             OnChanged(nameof(WartoscVAT), oldWartoscVAT, wartoscVAT);
             OnChanged(nameof(WartoscBrutto), oldWartoscBrutto, wartoscBrutto);
         }
     }

     
     public StatusFaktury Status
     {
         get => status;
         set => SetPropertyValue(nameof(Status), ref status, value);
     }


     [Action]
     public void Zatwierdz()
     {
         Status = StatusFaktury.Zatwierdzona;

     }

     [Action]
     public void Anuluj()
     {
         Status = StatusFaktury.Anulowana;
     }
 }

 public enum StatusFaktury
 {
     Przygotowana = 0, Zatwierdzona = 1, Anulowana = 9
 }

 [DomainComponent]
public class PodsumowanieVat{
     public StawkaVAT StawkaVat { get; set; }
     public decimal Netto { get; set; }
     public decimal Vat { get; set; }
     public decimal Brutto { get; set; }
 }
```


Aby dołożyć numerowanie faktur do klasy faktur dodamy w momencie zapisywania wyliczanie numeru faktury

```csharp
protected override void OnSaving()
{
    base.OnSaving();
    if (Session.IsNewObject(this))
    {
        if (String.IsNullOrEmpty(NumerFaktury))
        {
            int sequntialNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer,
                                                                        typeof(Faktura).FullName,
                                                                        DateTime.Now.Year.ToString());
            NumerFaktury = $"{DateTime.Now.Year}/{DateTime.Now.Month:00}/{sequntialNumber.ToString("D4"):C}";
        }
    }
}
```




## Towary

```csharp
[DefaultClassOptions]
   [DefaultProperty(nameof(Nazwa))]
   public class Produkt : XPObject
  {
     public Produkt(Session session) : base(session)
     { }


       string uwagi;
       decimal cena;
       StawkaVAT stawkaVAT;
       string eAN;
       string nazwa;
       string kod;

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Kod
       {
           get => kod;
           set => SetPropertyValue(nameof(Kod), ref kod, value);
       }

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Nazwa
       {
           get => nazwa;
           set => SetPropertyValue(nameof(Nazwa), ref nazwa, value);
       }

       [Size(11)]
       public string EAN
       {
           get => eAN;
           set => SetPropertyValue(nameof(EAN), ref eAN, value);
       }


       public StawkaVAT StawkaVAT
       {
           get => stawkaVAT;
           set => SetPropertyValue(nameof(StawkaVAT), ref stawkaVAT, value);
       }


       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public decimal Cena
       {
           get => cena;
           set => SetPropertyValue(nameof(Cena), ref cena, value);
       }
       
       [Size(SizeAttribute.Unlimited)]
       public string Uwagi
       {
           get => uwagi;
           set => SetPropertyValue(nameof(Uwagi), ref uwagi, value);
       }

   }
```
## Wpłaty

```csharp
public class Wplata : XPObject
{
    public Wplata(Session session) : base(session)
    { }


    string uwagi;
    RodzajPlatnosci rodzajPlatnosci;
    Waluta waluta;
    decimal kwotaWplaty;
    DateTime dataWplaty;
    Klient klient;


    [Association]
    public Klient Klient
    {
        get => klient;
        set => SetPropertyValue(nameof(Klient), ref klient, value);
    }

    public DateTime DataWplaty
    {
        get => dataWplaty;
        set => SetPropertyValue(nameof(DataWplaty), ref dataWplaty, value);
    }


    public decimal KwotaWplaty
    {
        get => kwotaWplaty;
        set => SetPropertyValue(nameof(KwotaWplaty), ref kwotaWplaty, value);
    }


    public Waluta Waluta
    {
        get => waluta;
        set => SetPropertyValue(nameof(Waluta), ref waluta, value);
    }


    public RodzajPlatnosci RodzajPlatnosci
    {
        get => rodzajPlatnosci;
        set => SetPropertyValue(nameof(RodzajPlatnosci), ref rodzajPlatnosci, value);
    }

    [Size(SizeAttribute.Unlimited)]
    public string Uwagi
    {
        get => uwagi;
        set => SetPropertyValue(nameof(Uwagi), ref uwagi, value);
    }
}

public enum RodzajPlatnosci
{
    Gotowka = 0, Karta = 1, BLIK = 3, Przelew = 4
}
```
