using Demo1.Module.BusinessObjects;
using DevExpress.Data.Filtering;
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


}
