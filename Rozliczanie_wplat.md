### Rozliczanie wpłat

Klient może pojedyncza wpłatę dokonąc na całą, część lub kilka faktur.
Może też wpłacić większa kwotę niż należy.
Dokonane wpłaty rejestrujemy do klasy **Wplaty**. Powiązanie wpłaty z fakturą przechowuje klasa **Rozrachunek**.

Standardowo wpłaty rozliczane są na zasadzie FIFO, niekiedy jednak klient może wskazać, że konkretna wpłata jest na poczet konkretnej faktury i na poczet tej faktury, należy zaliczyć wpłatę.

###
[wplata.cs](./ComphrehensiveTuririalXaf.Module/BusinessObjects/Wplata.cs)
[rozrachunek.cs](./ComphrehensiveTuririalXaf.Module/BusinessObjects/Rozrachunek.cs)


Tworze klasę NonPersistent, która wyswietli mi pierwsze okno z proponowana lista faktur

[WplataDoRozliczeniaDC.cs](./ComphrehensiveTuririalXaf.Module/BusinessObjects/WplataDoRozliczeniaDC.cs)

```csharp
public class WplataDoRozliczeniaDC : INotifyPropertyChanged
   {

       private void OnPropertyChanged(String propertyName)
       {
           if (PropertyChanged != null)
           {
               PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
           }
       }
       public event PropertyChangedEventHandler PropertyChanged;


       public WplataDoRozliczeniaDC()
       {
           rozrachunki = new BindingList<FakturaDoRozliczeniaDC>();
       }
       public Wplata Wplata { get; set; }
       private BindingList<FakturaDoRozliczeniaDC> rozrachunki;
       [ImmediatePostData]
       public BindingList<FakturaDoRozliczeniaDC> Naleznosci { get { return rozrachunki; } }

       //how to refresh this value when Naleznosci collection is changed
      // [PersistentAlias("Naleznosci.Sum(KwotaDoRozliczenia)")]
       public decimal SumaRozliczen
       {
           get
           {
               // return Convert.ToDecimal(EvaluateAlias(nameof(SumaRozliczen)));
               return Naleznosci.Sum(n => n.KwotaDoRozliczenia) ?? 0;
           }
       }
   }
```
