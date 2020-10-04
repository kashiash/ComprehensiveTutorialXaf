using ComprehensiveTutorialXaf.Module.Factory;
using Demo1.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComprehensiveTutorialXaf.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class FakturaKorygujaca : Faktura
    {
        public FakturaKorygujaca(Session session) : base(session)
        { }


        string powodKorekty;
        string uwagi;
        Faktura fakturaKorygowana;

        public Faktura FakturaKorygowana
        {
            get => fakturaKorygowana;
            set => SetPropertyValue(nameof(FakturaKorygowana), ref fakturaKorygowana, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PowodKorekty
        {
            get => powodKorekty;
            set => SetPropertyValue(nameof(PowodKorekty), ref powodKorekty, value);
        }



        XPCollection<PozycjaFaktury> pozycjePrzedKorekta;
        [XafDisplayName("Przed korektą")]
        public XPCollection<PozycjaFaktury> PozycjePrzedKorekta
        {
            get
            {
                if (pozycjePrzedKorekta == null)
                {
                    PrzygotujPozycjePrzedKorekta();
                    OnChanged(nameof(PozycjePrzedKorekta));
                }
                return pozycjePrzedKorekta;
            }
        }


        IList<PozycjaKorygujacaFakturyDC> pozycjePoKorekcie;
        [XafDisplayName("Po korekcie")]
        public IList<PozycjaKorygujacaFakturyDC> PozycjePoKorekcie
        {
            get
            {

                if (pozycjePoKorekcie == null)
                {
                    PrzygotujPozycjePoKorekcie();
                    OnChanged(nameof(PozycjePoKorekcie));
                }
                return pozycjePoKorekcie;

            }
        }

        private void PrzygotujPozycjePoKorekcie()
        {
            if (pozycjePoKorekcie == null)
            {
                pozycjePoKorekcie = new List<PozycjaKorygujacaFakturyDC>();
            }
            foreach (var korygowana in PozycjePrzedKorekta)
            {
                PozycjaKorygujacaFakturyDC oczekiwana = pozycjePoKorekcie
                    .Where(p => p.PozycjaKorygowana == korygowana)
                    .FirstOrDefault();
                if (oczekiwana == null)
                {
                    oczekiwana = new PozycjaKorygujacaFakturyDC()
                    {
                        Produkt = korygowana.Produkt,
                        Ilosc = korygowana.Ilosc,
                        CenaJednostkowaNetto = korygowana.Cena,
                        WartoscBrutto = korygowana.WartoscBrutto,
                        WartoscNetto = korygowana.WartoscNetto,
                        WartoscVAT = korygowana.WartoscVAT,
                        PozycjaKorygowana = korygowana,
                    };
                }
                pozycjePoKorekcie.Add(oczekiwana);

            }
            foreach (PozycjaFakturyKorygujacej korygujaca in PozycjeFaktury)
            {
                if (korygujaca != null)
                {
                    PozycjaKorygujacaFakturyDC oczekiwana = pozycjePoKorekcie
                        .Where(p => p.PozycjaKorygowana == korygujaca.PozycjaKorygowana)
                        .FirstOrDefault();
                    if (oczekiwana == null)
                    {
                        oczekiwana = new PozycjaKorygujacaFakturyDC()
                        {
                            Produkt = korygujaca.Produkt,
                            Ilosc = korygujaca.Ilosc,
                            CenaJednostkowaNetto = korygujaca.Cena,
                            WartoscBrutto = korygujaca.WartoscBrutto,
                            WartoscNetto = korygujaca.WartoscNetto,
                            WartoscVAT = korygujaca.WartoscVAT,
                            PozycjaKorygujaca = korygujaca,
                        };
                        pozycjePoKorekcie.Add(oczekiwana);
                    }
                    else
                    {
                        oczekiwana.Ilosc += korygujaca.Ilosc;
                        oczekiwana.CenaJednostkowaNetto += korygujaca.Cena;
                        oczekiwana.WartoscBrutto += korygujaca.WartoscBrutto;
                        oczekiwana.WartoscNetto += korygujaca.WartoscNetto;
                        oczekiwana.WartoscVAT += korygujaca.WartoscVAT;
                        oczekiwana.PozycjaKorygujaca = korygujaca;
                    }
                }
            }
        }

        private void PrzygotujPozycjePrzedKorekta()
        {
            pozycjePrzedKorekta = new XPCollection<PozycjaFaktury>(Session, CriteriaOperator.Parse("Faktura = ?", FakturaKorygowana));
        }

        [Size(SizeAttribute.Unlimited)]
        public string Uwagi
        {
            get => uwagi;
            set => SetPropertyValue(nameof(Uwagi), ref uwagi, value);
        }
    }
}
