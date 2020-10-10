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
okno to wywołuje z kontrolera na wpłatach:

```csharp
public class WplatyController : ViewController
  {
      public WplatyController()
      {
          TargetObjectType = typeof(Wplata);
          PopupWindowShowAction rozliczWplateAction =
              new PopupWindowShowAction(this, $"{GetType().FullName}.{nameof(rozliczWplateAction)}", PredefinedCategory.Edit)
              {
                  Caption = "Rozlicz wpłatę",
                  ImageName = "BO_Payment",
                  PaintStyle = ActionItemPaintStyle.Image,
                  ToolTip = "Przypisuje wpłate do nierozliczonych faktur",
                  ConfirmationMessage = "Jesteś pewny tego czynu?",
                  SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
              };
          rozliczWplateAction.CustomizePopupWindowParams += rozliczWplateAction_CustomizePopupWindowParams;
          rozliczWplateAction.Execute += rozliczWplateAction_Execute;
      }

      private void rozliczWplateAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
      {



          IObjectSpace objectSpace = Application.CreateObjectSpace();
          e.Context = TemplateContext.PopupWindow;
          var rozliczenie = new WplataDoRozliczeniaDC();
          rozliczenie.Wplata = (Wplata)objectSpace.GetObject(View.CurrentObject);
          PrzygotujListeFakturDoRozliczenia(objectSpace,rozliczenie);
          e.View = Application.CreateDetailView(objectSpace, rozliczenie);
          ((DetailView)e.View).ViewEditMode = ViewEditMode.Edit;
      }
      private void rozliczWplateAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
      {
        //  IObjectSpace objectSpace = Application.CreateObjectSpace();
          var wplata = (Wplata)ObjectSpace.GetObject(View.CurrentObject);
          if (wplata != null)
          { WplataDoRozliczeniaDC parameters = e.PopupWindow.View.CurrentObject as WplataDoRozliczeniaDC;
              foreach (var obj in parameters.Naleznosci)
              {
                  var roz = ObjectSpace.GetObject(obj);
                  if (roz.KwotaDoRozliczenia != 0)
                  {
                      var rozrachunek = ObjectSpace.CreateObject<Rozrachunek>();
                      rozrachunek.Wplata = wplata;
                      rozrachunek.Faktura = ObjectSpace.GetObject(roz.Faktura); 
                      rozrachunek.Kwota = roz.KwotaDoRozliczenia;
                  }

              }
          }
          //if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
          //{
          //    objectSpace.CommitChanges();
          //}
          if (View is ListView)
          {
              ObjectSpace.CommitChanges();
              View.ObjectSpace.Refresh();
          }
      }

      private void PrzygotujListeFakturDoRozliczenia(IObjectSpace objectSpace, WplataDoRozliczeniaDC rozliczenie)
      {

 
        RozliczanieWplatHelper.WyszukajFaktury(objectSpace,rozliczenie);

      }



      protected override void OnActivated()
      {
          base.OnActivated();
      }
      protected override void OnDeactivated()
      {
          base.OnDeactivated();
      }
  }
```

Okno do wybierania faktur potrzebuje 3 akcje
* znajdz faktury do rozliczenia - Simple Action
* wyczyśc liste faktur do rozliczenia - Simple Action
* wybierz faktury z listy - PopupWindowAction



o ile 2 piersze akcje sa proste o tyle dla 3ciej trzeba przygotowac kolekcje faktur dla klienta, które nie są rozliczone, wyswietlic ta listę i pozwolić klientowi wybrac dowolne faktury z listy.
Po zamknieciu okna przypisac pozycje do listy rozrachunków.

```csharp
public class WplataDoRozliczeniaDCDetailViewController :ObjectViewController<DetailView, WplataDoRozliczeniaDC>

 {

     SimpleAction PrzypiszFaktury;
     SimpleAction WyczyscListeFaktur;
     PopupWindowShowAction WybierzFakturyAction;
     public WplataDoRozliczeniaDCDetailViewController()
     {
         PrzypiszFaktury = new SimpleAction(
             this, $"{GetType().FullName}.{nameof(PrzypiszFaktury)}",
             DevExpress.Persistent.Base.PredefinedCategory.Edit)
         { 
         Caption = "Przypisz faktury",
         ImageName = "Action_Search",


         };
         PrzypiszFaktury.Execute += PrzypiszFaktury_Execute;

         WyczyscListeFaktur = new SimpleAction(
             this,
             $"{GetType().FullName}.{nameof(WyczyscListeFaktur)}",
             DevExpress.Persistent.Base.PredefinedCategory.Edit) {
             Caption = "Wyczyść listę faktur",
             ImageName = "Action_Clear",
         };
         WyczyscListeFaktur.Execute += WyczyscListeFaktur_Execute;

         WybierzFakturyAction = new PopupWindowShowAction(this, $"{GetType().FullName}.{nameof(WybierzFakturyAction)}", DevExpress.Persistent.Base.PredefinedCategory.Edit) { 
         Caption = "Wybierz faktury",
         ImageName= "BO_Invoice",
         };
         WybierzFakturyAction.CustomizePopupWindowParams += WybierzFakturyAction_CustomizePopupWindowParams;
         WybierzFakturyAction.Execute += WybierzFakturyAction_Execute;
     }



     private void WybierzFakturyAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
     {
    
         IObjectSpace objectSpace = Application.CreateObjectSpace();
         var roz = objectSpace.GetObject(ViewCurrentObject);
         var kli = objectSpace.GetObject(roz.Wplata.Klient);
         CriteriaOperator co =  CriteriaOperator.Parse("WartoscBrutto > SumaWplat And Klient = ?", kli);
         CollectionSourceBase collectionSource = new CollectionSource(objectSpace,typeof(Faktura));
      
         collectionSource.Criteria["FakturyDoSplaty"] = co; 
        string fakturaListViewId = Application.FindLookupListViewId(typeof(Faktura));
         e.View = Application.CreateListView(fakturaListViewId, collectionSource, true);
     }

     private void WybierzFakturyAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
     {
         var rozliczenie = ViewCurrentObject;
         foreach (var obj in e.PopupWindowViewSelectedObjects)
         {

             var faktura = (Faktura)ObjectSpace.GetObject(obj);
             var fakturaDoRozliczenia = new FakturaDoRozliczeniaDC();
             fakturaDoRozliczenia.Faktura = faktura;
     
             var kwotaDoRozliczenia = rozliczenie.Wplata.Nadplata - rozliczenie.SumaRozliczen;

             var kwotaFaktury = faktura.WartoscBrutto - faktura.SumaWplat;
             var kwotaDoFaktury = kwotaDoRozliczenia > kwotaFaktury ? kwotaFaktury  : kwotaDoRozliczenia;

             fakturaDoRozliczenia.KwotaDoRozliczenia = kwotaDoFaktury;

             rozliczenie.Naleznosci.Add(fakturaDoRozliczenia);
          //   rozliczenie.SumaRozliczen += kwotaDoFaktury ;
         }
         View.Refresh();
     }

     private void WyczyscListeFaktur_Execute(object sender, SimpleActionExecuteEventArgs e)
     {
         var rozliczenie = ViewCurrentObject;
         rozliczenie.Naleznosci.Clear();
         View.Refresh();
     }

     private void PrzypiszFaktury_Execute(object sender, SimpleActionExecuteEventArgs e)
     {
         var kwotaDoRozliczenia = RozliczanieWplatHelper.WyszukajFaktury(ObjectSpace, ViewCurrentObject);
         View.Refresh();
     }
 }
```



