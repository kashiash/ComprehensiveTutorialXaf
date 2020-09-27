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
    public class StawkaVAT : XPLiteObject
	{
		public StawkaVAT(Session session) : base(session)
		{ }


        decimal stawka;

        string symbol;

        [Size(3)]
        [Key(false)]
        public string Symbol
        {
            get => symbol;
            set => SetPropertyValue(nameof(Symbol), ref symbol, value);
        }
        
        public decimal Stawka
        {
            get => stawka;
            set => SetPropertyValue(nameof(Stawka), ref stawka, value);
        }
    }
}
