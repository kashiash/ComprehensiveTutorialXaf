# Faktury, pozycje i wpłaty

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
