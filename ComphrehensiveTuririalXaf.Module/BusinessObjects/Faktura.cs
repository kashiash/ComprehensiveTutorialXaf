﻿using ComprehensiveTutorialXaf.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo1.Module.BusinessObjects
{
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



    
        decimal sumaWplat;
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

        internal void ZmienKlienta(Klient newCustomer)
        {
            var oldKlient = klient;
       
            if (oldKlient != null && oldKlient.IsNewObject  && oldKlient != newCustomer)
            {
                oldKlient.Delete();
            }

            Klient = newCustomer;
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
        [EditorAlias(EditorAliases.DetailPropertyEditor)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public Klient Klient
        {
            get => klient;
            set
            {
                var oldKlient = klient;

                var modified = SetPropertyValue(nameof(Klient), ref klient, value);

                if (modified && !IsLoading && !IsSaving && Klient != null)
                {

                   

                    if (oldKlient != null && IsNewObject(oldKlient) && oldKlient != Klient)
                    {
                        oldKlient.Delete();
                    }
                    DataPlatnosci = DataFaktury.AddDays(Klient.TerminPlatnosci);

                }
            }
        }

        private bool IsNewObject(Klient obj)
        {
            IObjectSpace objectSpace = XPObjectSpace.FindObjectSpaceByObject(obj);
            return objectSpace != null && objectSpace.IsNewObject(obj);
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
        decimal wartoscVAT;
        [PersistentAlias(nameof(wartoscVAT))]
        public decimal WartoscVAT
        {
            get => wartoscVAT;
            set => SetPropertyValue(nameof(WartoscVAT), ref wartoscVAT, value);
        }
        [Persistent("WartoscBrutto")]
        decimal wartoscBrutto;
        [PersistentAlias(nameof(wartoscBrutto))]
        public decimal WartoscBrutto
        {
            get => wartoscBrutto;
            set => SetPropertyValue(nameof(WartoscBrutto), ref wartoscBrutto, value);
        }


        public decimal SumaWplat
        {
            get => sumaWplat;
            set => SetPropertyValue(nameof(SumaWplat), ref sumaWplat, value);
        }


        FakturaKorygujaca fakturaKorygujaca;
        public FakturaKorygujaca FakturaKorygujaca
        {
            get => fakturaKorygujaca;
            set => SetPropertyValue(nameof(FakturaKorygujaca), ref fakturaKorygujaca, value);
        }

        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<PozycjaFaktury> PozycjeFaktury
        {
            get
            {
                return GetCollection<PozycjaFaktury>(nameof(PozycjeFaktury));
            }
        }

        [Association("Faktura-Rozrachunki"), DevExpress.Xpo.Aggregated]
        public XPCollection<Rozrachunek> Rozrachunki
        {
            get
            {
                return GetCollection<Rozrachunek>(nameof(Rozrachunki));
            }
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();

            // ustawiamy wartości poczatkowe
            // Opowiednik w clarionie "On prime records"
            DataFaktury = DateTime.Now;
            Status = StatusFaktury.Przygotowana;

            Klient = new Klient(Session);

        }

        List<PodsumowanieVat> podsumowanieVat;
        [Delayed]
        public List<PodsumowanieVat> ListaVat
        {
            get
            {
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


        internal void PrzeliczWplaty(bool forceChangeEvents)
        {
            decimal oldSumaWplat = sumaWplat;
            decimal tmpSumaWplat = 0m;
            foreach (var rozrachunek in Rozrachunki)
            {
                tmpSumaWplat += rozrachunek.Kwota;
            }
            sumaWplat = tmpSumaWplat;

            if (forceChangeEvents)
            {
                OnChanged(nameof(SumaWplat), oldSumaWplat, sumaWplat);
            }
        }

        public void PrzeliczSumy(bool forceChangeEvents = true)
        {
            decimal oldWartoscNetto = wartoscNetto;
            decimal? oldWartoscVAT = wartoscVAT;
            decimal? oldWartoscBrutto = wartoscBrutto;


            decimal tmpWartoscNetto = 0m;
            decimal tmpWartoscVAT = 0m;
            decimal tmpWartoscBrutto = 0m;

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


        protected override void OnSaving()
        {
            base.OnSaving();

            if (Session.IsNewObject(this))
            {

                if (String.IsNullOrEmpty(NumerFaktury))
                {
                    int sequntialNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer,
                                                                                typeof(Faktura).FullName,
                                                                                $"{DateTime.Now.Year}");

                    NumerFaktury = $"{DateTime.Now.Year}/{DateTime.Now.Month:00}/{sequntialNumber.ToString("D4"):C}";
                }
            }
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
    public class PodsumowanieVat
    {
        public StawkaVAT StawkaVat { get; set; }
        public decimal Netto { get; set; }
        public decimal Vat { get; set; }
        public decimal Brutto { get; set; }
    }
}
