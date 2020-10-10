using Demo1.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.BusinessObjects
{
    public class Rozrachunek : XPObject
    {
        public Rozrachunek(Session session) : base(session)
        { }



        decimal kwota;
        Faktura faktura;
        Wplata wplata;


        [Association("Wplata-Rozrachunki")]
        public Wplata Wplata
        {
            get => wplata;
            set
            {
               
                bool modified = SetPropertyValue(nameof(Wplata), ref wplata, value);
                if (!IsLoading && !IsSaving && Wplata != null && modified)
                {
                    Wplata.PrzeliczRozrachunki(true);

                }

            }
        }

        [Association("Faktura-Rozrachunki")]
        public Faktura Faktura
        {
            get => faktura;
            set
            {
                bool modified = SetPropertyValue(nameof(Faktura), ref faktura, value);
             
                if (!IsLoading && !IsSaving && Faktura != null && modified)
                {
                    Faktura.PrzeliczWplaty(true);

                }
            }
        }

        
        public decimal Kwota
        {
            get => kwota;
            set => SetPropertyValue(nameof(Kwota), ref kwota, value);
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (propertyName == nameof(Kwota))
            {
                if (Faktura != null)
                    Faktura.PrzeliczWplaty(true);
                if (Wplata != null)
                    Wplata.PrzeliczRozrachunki(true);
            }
        }
    }
}
