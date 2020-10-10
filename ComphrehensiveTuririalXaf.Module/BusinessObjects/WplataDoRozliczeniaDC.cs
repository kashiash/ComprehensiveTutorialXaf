using Demo1.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.BusinessObjects
{
    [DomainComponent]
    public class WplataDoRozliczeniaDC : NonPersistentBaseObject, INotifyPropertyChanged
    {




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
                return Naleznosci.Sum(n => n.KwotaDoRozliczenia) ;
            }
        }


        private void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public static class RozliczanieWplatHelper
    {

        public static decimal? WyszukajFaktury(
            IObjectSpace objectSpace,
            WplataDoRozliczeniaDC rozliczenie
          )
        {
            var faktury = objectSpace.GetObjectsQuery<Faktura>()
                                     .Where(f => f.Klient == rozliczenie.Wplata.Klient
                                                 && f.SumaWplat < f.WartoscBrutto);
            decimal kwotaDoRozliczenia = rozliczenie.Wplata.Nadplata ;
            foreach (var faktura in faktury)
            {
                //var pozycja = objectSpace.CreateObject<FakturaDoRozliczenia>(); // to nie zadziała bo faktura do rozliczania nie jest XPO !!!!
                var pozycja = new FakturaDoRozliczeniaDC();
                pozycja.Faktura = faktura;
                var kwotaFaktury = faktura.WartoscBrutto - faktura.SumaWplat;
                if (kwotaDoRozliczenia > kwotaFaktury)
                {
                    pozycja.KwotaDoRozliczenia = kwotaFaktury;
                    kwotaDoRozliczenia -= kwotaFaktury;
                }
                else
                {
                    pozycja.KwotaDoRozliczenia = kwotaDoRozliczenia;
                    kwotaDoRozliczenia = 0;
                }
                rozliczenie.Naleznosci.Add(pozycja);

            }

            return kwotaDoRozliczenia;
        }
    }
}
