using ComprehensiveTutorialXaf.Module.BusinessObjects;
using Demo1.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;

using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Factory
{
    public class InvoiceFactory
    {
        IObjectSpace os;
        public InvoiceFactory(IObjectSpace _os)
        {
            os = _os;
        }




        internal Faktura UtworzFakture(string[] towary)
        {

            var faktura = os.CreateObject<Faktura>();
            foreach (var towar in towary)
            {
                var pozycja = os.CreateObject<PozycjaFaktury>();

                var produkt = os.FindObject<Produkt>(new BinaryOperator(nameof(Produkt.Nazwa), towar));
                if (produkt != null)
                {
                    pozycja.Produkt = produkt;
                    pozycja.Ilosc = 1;
                    pozycja.Faktura = faktura;
                    // uwaga co ciekawe to nie zadziała !!!! po dodaniu w momencie przypsiywania faktury kolekcja jest jeszcze pusta
                    faktura.PozycjeFaktury.Add(pozycja);
                    Console.WriteLine($"Netto poz {pozycja.WartoscNetto}, fakt {faktura.WartoscNetto}");
                }
            }

            Console.WriteLine($"Faktura {faktura.WartoscNetto}, fakt {faktura.WartoscBrutto}");
            faktura.PrzeliczSumy(); // to jest z powodu błedy ?? w XPO

            return faktura;
        }


        internal Faktura UtworzFakture(IList<PozycjaFakturyDC> towary)
        {

            var faktura = os.CreateObject<Faktura>();
            foreach (var towar in towary)
            {
                var pozycja = os.CreateObject<PozycjaFaktury>();

                var produkt = os.GetObject(towar.Produkt);
                if (produkt != null)
                {
                    pozycja.Produkt = produkt;
                    pozycja.Ilosc = towar.Ilosc;
                    pozycja.Cena = towar.CenaJednostkowaNetto ?? produkt.Cena;
                    pozycja.Faktura = faktura;
                    // uwaga co ciekawe to nie zadziała !!!! po dodaniu w momencie przypsiywania faktury kolekcja jest jeszcze pusta
                    faktura.PozycjeFaktury.Add(pozycja);
                    Console.WriteLine($"Netto poz {pozycja.WartoscNetto}, fakt {faktura.WartoscNetto}");
                }
            }

            Console.WriteLine($"Faktura {faktura.WartoscNetto}, fakt {faktura.WartoscBrutto}");
            faktura.PrzeliczSumy(); // to jest z powodu błedy ?? w XPO

            return faktura;
        }

        public FakturaKorygujaca UtworzKorekteCalkowita(Faktura faktura)
        {
            var korekta = os.CreateObject<FakturaKorygujaca>();
            korekta.FakturaKorygowana = faktura;
            faktura.FakturaKorygujaca = korekta;
            foreach (var poz in faktura.PozycjeFaktury)
            {
                var kor = os.CreateObject<PozycjaFakturyKorygujacej>();

                kor.PozycjaKorygowana = poz;
                poz.PozycjaKorygujaca = kor;
                kor.Produkt = poz.Produkt;
                kor.WartoscBrutto = poz.WartoscBrutto * -1;
                kor.WartoscNetto = poz.WartoscNetto * -1;
                kor.WartoscVAT = poz.WartoscVAT * -1;
                kor.Ilosc = poz.Ilosc * -1;


                kor.Faktura = korekta;
            }
            return korekta;
        }

        internal FakturaKorygujaca UtworzKorekte(Faktura faktura, IList<PozycjaKorygujacaFakturyDC> listaPozycjiKorygujacych)
        {
            if (listaPozycjiKorygujacych == null || listaPozycjiKorygujacych.Count == 0)
            {
                return null;
            }

            var korekta = os.CreateObject<FakturaKorygujaca>();
            korekta.FakturaKorygowana = faktura;
            faktura.FakturaKorygujaca = korekta;

            foreach (var oczPozKorygujaca in listaPozycjiKorygujacych)
            {
                var pozycjaDoSkorygowania = faktura.PozycjeFaktury.Where(p => p.Produkt == oczPozKorygujaca.Produkt).FirstOrDefault();
                var pozycjaKorygujaca = os.CreateObject<PozycjaFakturyKorygujacej>();
                pozycjaKorygujaca.Produkt = oczPozKorygujaca.Produkt;

                pozycjaKorygujaca.PozycjaKorygowana = pozycjaDoSkorygowania;
                if (pozycjaDoSkorygowania != null)
                {
                    pozycjaDoSkorygowania.PozycjaKorygujaca = pozycjaKorygujaca;

                }

                pozycjaKorygujaca.WartoscBrutto = pozycjaDoSkorygowania != null ? (oczPozKorygujaca.WartoscBrutto ?? 0) - pozycjaDoSkorygowania.WartoscBrutto : (oczPozKorygujaca.WartoscBrutto ?? 0);
                pozycjaKorygujaca.WartoscNetto = pozycjaDoSkorygowania != null ? (oczPozKorygujaca.WartoscNetto ?? 0) - pozycjaDoSkorygowania.WartoscNetto : (oczPozKorygujaca.WartoscNetto ?? 0);
                pozycjaKorygujaca.WartoscVAT = pozycjaDoSkorygowania != null ? (oczPozKorygujaca.WartoscVAT ?? 0) - pozycjaDoSkorygowania.WartoscVAT : (oczPozKorygujaca.WartoscVAT ?? 0);
                pozycjaKorygujaca.Ilosc = pozycjaDoSkorygowania != null ? oczPozKorygujaca.Ilosc - pozycjaDoSkorygowania.Ilosc : oczPozKorygujaca.Ilosc;


                pozycjaKorygujaca.Faktura = korekta;

            }
            return korekta;
        }
    }
    [DomainComponent]
    public class PozycjaFakturyDC
    {

        public Produkt Produkt { get; set; }
        public decimal Ilosc { get; set; }
        public decimal? CenaJednostkowaNetto { get; set; }

    }

    [DomainComponent]
    public class PozycjaKorygujacaFakturyDC : PozycjaFakturyDC
    {
        public decimal? WartoscNetto { get; set; }
        public decimal? WartoscVAT { get; set; }
        public decimal? WartoscBrutto { get; set; }
    }

}
