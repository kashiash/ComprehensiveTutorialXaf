using ComprehensiveTutorialXaf.Module.BusinessObjects;
using Demo1.Module.BusinessObjects;
using DevExpress.ExpressApp;
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

                kor.WartoscBrutto = poz.WartoscBrutto * -1;
                kor.WartoscNetto = poz.WartoscNetto * -1;
                kor.WartoscVAT = poz.WartoscVAT * -1;
                kor.Ilosc = poz.Ilosc * -1;


                kor.Faktura = korekta;
            }
            return korekta;
        }
    }
}
