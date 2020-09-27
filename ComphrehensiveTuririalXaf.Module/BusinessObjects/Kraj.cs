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
    [DefaultProperty(nameof(Symbol))]
    public class Kraj : XPCustomObject
	{
		public Kraj(Session session) : base(session)
		{ }

        
        [Size(3)]
        [Key]
        public string Symbol
        {
            get => symbol;
            set => SetPropertyValue(nameof(Symbol), ref symbol, value);
        }

        bool isMetric;
        Waluta waluta;
        int geoId;
        string symbol;
        string nazwa;



        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nazwa
        {
            get => nazwa;
            set => SetPropertyValue(nameof(Nazwa), ref nazwa, value);
        }

        string lokalnaNazwa;
        string lokalnySymbol;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LokalnySymbol
        {
            get => lokalnySymbol;
            set => SetPropertyValue(nameof(LokalnySymbol), ref lokalnySymbol, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LokalnaNazwa
        {
            get => lokalnaNazwa;
            set => SetPropertyValue(nameof(LokalnaNazwa), ref lokalnaNazwa, value);
        }

        public int GeoId
        {
            get => geoId;
            set => SetPropertyValue(nameof(GeoId), ref geoId, value);
        }


        public Waluta Waluta
        {
            get => waluta;
            set => SetPropertyValue(nameof(Waluta), ref waluta, value);
        }
        
        public bool IsMetric
        {
            get => isMetric;
            set => SetPropertyValue(nameof(IsMetric), ref isMetric, value);
        }
    }
}
