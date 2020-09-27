using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo1.Module.BusinessObjects
{
    public class Wplata : XPObject
    {
        public Wplata(Session session) : base(session)
        { }


        string uwagi;
        RodzajPlatnosci rodzajPlatnosci;
        Waluta waluta;
        decimal kwotaWplaty;
        DateTime dataWplaty;
        Klient klient;


        [Association]
        public Klient Klient
        {
            get => klient;
            set => SetPropertyValue(nameof(Klient), ref klient, value);
        }

        public DateTime DataWplaty
        {
            get => dataWplaty;
            set => SetPropertyValue(nameof(DataWplaty), ref dataWplaty, value);
        }


        public decimal KwotaWplaty
        {
            get => kwotaWplaty;
            set => SetPropertyValue(nameof(KwotaWplaty), ref kwotaWplaty, value);
        }


        public Waluta Waluta
        {
            get => waluta;
            set => SetPropertyValue(nameof(Waluta), ref waluta, value);
        }


        public RodzajPlatnosci RodzajPlatnosci
        {
            get => rodzajPlatnosci;
            set => SetPropertyValue(nameof(RodzajPlatnosci), ref rodzajPlatnosci, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Uwagi
        {
            get => uwagi;
            set => SetPropertyValue(nameof(Uwagi), ref uwagi, value);
        }
    }

    public enum RodzajPlatnosci
    {
        Gotowka = 0, Karta = 1, BLIK = 3, Przelew = 4
    }

}
