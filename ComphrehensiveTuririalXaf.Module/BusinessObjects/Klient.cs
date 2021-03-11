using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using Bogus;
using ComprehensiveTutorialXaf.Module.BusinessObjects;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace Demo1.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Customer")]
    [DefaultProperty(nameof(Skrot))]
    public class Klient : BaseObject
    {
        public Klient(Session session)
              : base(session)
        {
        }



        AdresKlienta adresKorespondencyjny;
        AdresKlienta adresSiedziby;
        int terminPlatnosci;

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


        [EditorAlias(EditorAliases.DetailPropertyEditor)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public AdresKlienta AdresSiedziby
        {
            get => adresSiedziby;
            set
            {
                var oldAdres = adresSiedziby;
                bool modified = SetPropertyValue(nameof(AdresSiedziby), ref adresSiedziby, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    if (oldAdres != null && oldAdres != AdresSiedziby &&  oldAdres.IsNewObject)
                    {
                        oldAdres.Delete();
                    }
                    AdresyKlienta.Add(AdresSiedziby);

                }
            }
        }
        private bool _innyAdresKorespondecyjny;
        [XafDisplayName("Adres korespondecyjny jest inny")]
        [ImmediatePostData]
        public bool InnyAdresKorespondecyjny
        {
            get => _innyAdresKorespondecyjny;
            set
            {
                bool modified = SetPropertyValue(nameof(InnyAdresKorespondecyjny), ref _innyAdresKorespondecyjny, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    if (InnyAdresKorespondecyjny)
                    {
                        AdresKorespondencyjny = new AdresKlienta(Session);
                    }
                    else
                    {
                        if (AdresKorespondencyjny.IsNewObject)
                        {
                            AdresKorespondencyjny.Delete();
                        }
                        AdresKorespondencyjny = null;
                    }
                }
            }
        }

        [Browsable(false)]
        [NonPersistent]
        public bool IsNewObject
        {
            get;set;
        }

        private bool UkryjAdresKorespondencyjny => !InnyAdresKorespondecyjny;

        [EditorAlias(EditorAliases.DetailPropertyEditor)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]

        [Appearance(nameof(UkryjAdresKorespondencyjny), Visibility = ViewItemVisibility.Hide, Criteria = nameof(UkryjAdresKorespondencyjny))]
        public AdresKlienta AdresKorespondencyjny
        {
            get => adresKorespondencyjny;
            set
            {
                var oldAdres = adresKorespondencyjny;
                bool modified = SetPropertyValue(nameof(AdresKorespondencyjny), ref adresKorespondencyjny, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    if (oldAdres != null && oldAdres != AdresKorespondencyjny  && oldAdres.IsNewObject)
                    {
                        oldAdres.Delete();
                    }
                    AdresyKlienta.Add(AdresKorespondencyjny);

                }
            }
        }

        [Association("Klient-AdresyKlienta")]
        public XPCollection<AdresKlienta> AdresyKlienta
        {
            get
            {
                return GetCollection<AdresKlienta>(nameof(AdresyKlienta));
            }
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TerminPlatnosci = 14;
            IsNewObject = true;
        }



    }
}