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
    public class Waluta : XPCustomObject
	{
		public Waluta(Session session) : base(session)
		{ }


        Kraj kraj;
        string symbol;
        string nazwa;


        [Size(3)]
        [Key]
        public string Symbol
        {
            get => symbol;
            set => SetPropertyValue(nameof(Symbol), ref symbol, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nazwa
        {
            get => nazwa;
            set => SetPropertyValue(nameof(Nazwa), ref nazwa, value);
        }

        public Kraj Kraj
        {
            get => kraj;
            set => SetPropertyValue(nameof(Kraj), ref kraj, value);
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
    }
}
