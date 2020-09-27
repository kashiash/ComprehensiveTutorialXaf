# Klienci

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
