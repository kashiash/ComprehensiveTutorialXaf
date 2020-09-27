# Klienci


Prosze sie nie przywiązywac do tej struktury, w kolejnych przykładach dokonamy na niej pewnej refaktoryzacji. Jest ona przydatna dla mniejszych aplikacji i nie trzeba jej zmieniac, nie mniej jednak gdy będziemy chcieli rozbudowac go o Dostawców, kLientów firmy wspołpracujace itd, to warto poczytac dalej  o refaktoryzacji do rozdzielenia adresów organizacji itp ...

```csharp
[DefaultClassOptions]
   [ImageName("BO_Customer")]
   [DefaultProperty(nameof(Skrot))]
public class Klient : BaseObject
   { 
     public Klient(Session session)
           : base(session)
       {
       }



       int terminPlatnosci;
       string miejscowosc;
       string kodPocztowy;
       string ulica;
       string telefon;
       string email;
       string skrot;
       string nazwa;

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Nazwa
       {
           get => nazwa;
           set => SetPropertyValue(nameof(Nazwa), ref nazwa, value);
       }

       [RuleRequiredField(DefaultContexts.Save)]

       [RuleUniqueValue]
       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Skrot
       {
           get => skrot;
           set => SetPropertyValue(nameof(Skrot), ref skrot, value);
       }


       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Email
       {
           get => email;
           set => SetPropertyValue(nameof(Email), ref email, value);
       }


       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Telefon
       {
           get => telefon;
           set => SetPropertyValue(nameof(Telefon), ref telefon, value);
       }


       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Ulica
       {
           get => ulica;
           set => SetPropertyValue(nameof(Ulica), ref ulica, value);
       }

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string KodPocztowy
       {
           get => kodPocztowy;
           set => SetPropertyValue(nameof(KodPocztowy), ref kodPocztowy, value);
       }

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Miejscowosc
       {
           get => miejscowosc;
           set => SetPropertyValue(nameof(Miejscowosc), ref miejscowosc, value);
       }


       
       public int TerminPlatnosci
       {
           get => terminPlatnosci;
           set => SetPropertyValue(nameof(TerminPlatnosci), ref terminPlatnosci, value);
       }




       [XafDisplayName("Kontakty")]
       [Association("Klient-Kontakty"), DevExpress.Xpo.Aggregated]
       public XPCollection<Kontakt> Kontakty
       {
           get
           {
               return GetCollection<Kontakt>(nameof(Kontakty));
           }
       }

       [Association]
       public XPCollection<Spotkanie> Spotkania
       {
           get
           {
               return GetCollection<Spotkanie>(nameof(Spotkania));
           }
       }

       [Association]
       public XPCollection<Faktura> Faktury
       {
           get
           {
               return GetCollection<Faktura>(nameof(Faktury));
           }
       }

       [Association]
       public XPCollection<Wplata> Wplaty
       {
           get
           {
               return GetCollection<Wplata>(nameof(Wplaty));
           }
       }

       public override void AfterConstruction()
       {
           base.AfterConstruction();
           TerminPlatnosci = 14;
       }



   }
```
 ### Kontakty
 
 ```csharp
[DefaultClassOptions]
[XafDefaultProperty(nameof(Nazwisko))]
[ImageName("BO_Contact")]
[Appearance("KontaktArchiwalny",Criteria = "Archiwalny = true",TargetItems = "*", FontColor  = "Gray")]
public class Kontakt : XPObject
{
    public Kontakt(Session session) : base(session)
    { }

    [VisibleInListView(false)]
    [VisibleInDetailView(false)]
    public string FullName
    {
        get
        {
            return ObjectFormatter.Format("{Imie} {Nazwisko}", this, EmptyEntriesMode.RemoveDelimeterWhenEntryIsEmpty);
        }
    }


    string email;
    Klient klient;
    string telefon;
    string nazwisko;
    string imie;
    bool archiwalny;
    Stanowisko stanowisko;

    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Imie
    {
        get => imie;
        set => SetPropertyValue(nameof(Imie), ref imie, value);
    }

    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Nazwisko
    {
        get => nazwisko;
        set => SetPropertyValue(nameof(Nazwisko), ref nazwisko, value);
    }

    
    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Email
    {
        get => email;
        set => SetPropertyValue(nameof(Email), ref email, value);
    }

    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Telefon
    {
        get => telefon;
        set => SetPropertyValue(nameof(Telefon), ref telefon, value);
    }


    public Stanowisko Stanowisko
    {
        get => stanowisko;
        set => SetPropertyValue(nameof(Stanowisko), ref stanowisko, value);
    }

    public bool Archiwalny
    {
        get => archiwalny;
        set => SetPropertyValue(nameof(Archiwalny), ref archiwalny, value);
    }


    [Association("Klient-Kontakty")]
    public Klient Klient
    {
        get => klient;
        set => SetPropertyValue(nameof(Klient), ref klient, value);
    }

    [Action(Caption = "Ustaw jako aktywny", ConfirmationMessage = "Are you sure?", ImageName = "BO_Active", AutoCommit = true)]
    public void ActiveActionMethod()
    {
        // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        this.Archiwalny = false;
    }
    [Action(Caption = "Ustaw jako archiwalny", ConfirmationMessage = "Are you sure?", ImageName = "BO_Inactive", AutoCommit = true)]
    public void ArchiveActionMethod()
    {
        // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        this.Archiwalny = true;
    }
}
```


### Spotkania

Spotkania dziedziczymy z Event, predefiniowanej biblioteki dostarczonej przez Devexpress. Zyskujemy dzieki temu obsługę w Scheluer Module z ładnym kalendarzem/plannerem

```csharp
[DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]

    public class Spotkanie : Event
    {
     public Spotkanie(Session session)
            : base(session)
        {
        }


        Kontakt osoba;

        Klient klient;

        [Association]
        public Klient Klient
        {
            get => klient;
            set => SetPropertyValue(nameof(Klient), ref klient, value);
        }


        [DataSourceProperty("Klient.Kontakty")]
        public Kontakt Osoba
        {
            get => osoba;
            set => SetPropertyValue(nameof(Osoba), ref osoba, value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }

    }
```
