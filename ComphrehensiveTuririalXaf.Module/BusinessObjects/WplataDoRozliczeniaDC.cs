using Demo1.Module.BusinessObjects;
using DevExpress.ExpressApp.DC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.BusinessObjects
{
    [DomainComponent]
    public class WplataDoRozliczeniaDC
    {
        public WplataDoRozliczeniaDC()
        {
            rozrachunki = new BindingList<FakturaDoRozliczeniaDC>();
        }
        public Wplata Wplata { get; set; }
        private BindingList<FakturaDoRozliczeniaDC> rozrachunki;
        public BindingList<FakturaDoRozliczeniaDC> Naleznosci { get { return rozrachunki; } }

        //how to refresh this value when Naleznosci collection is changed
        [Calculated("Naleznosci.Sum(KwotaDoRozliczenia)")]
        public decimal SumaRozliczen { get; }
    }

    [DomainComponent]
    public class FakturaDoRozliczeniaDC
    {
        public FakturaDoRozliczeniaDC()
        {

        }
        public Faktura Faktura { get; set; }
        public decimal? KwotaDoRozliczenia { get; set; }
    }
}
