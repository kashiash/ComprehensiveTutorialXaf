using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo1.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(Nazwa))]
    public class Produkt : XPObject
	{
		public Produkt(Session session) : base(session)
		{ }


        string uwagi;
        decimal cena;
        StawkaVAT stawkaVAT;
        string eAN;
        string nazwa;
        string kod;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Kod
        {
            get => kod;
            set => SetPropertyValue(nameof(Kod), ref kod, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nazwa
        {
            get => nazwa;
            set => SetPropertyValue(nameof(Nazwa), ref nazwa, value);
        }

        [Size(11)]
        public string EAN
        {
            get => eAN;
            set => SetPropertyValue(nameof(EAN), ref eAN, value);
        }


        public StawkaVAT StawkaVAT
        {
            get => stawkaVAT;
            set => SetPropertyValue(nameof(StawkaVAT), ref stawkaVAT, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public decimal Cena
        {
            get => cena;
            set => SetPropertyValue(nameof(Cena), ref cena, value);
        }
        
        [Size(SizeAttribute.Unlimited)]
        public string Uwagi
        {
            get => uwagi;
            set => SetPropertyValue(nameof(Uwagi), ref uwagi, value);
        }

    }
}
