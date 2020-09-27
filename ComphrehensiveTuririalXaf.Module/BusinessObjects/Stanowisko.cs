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
	public class Stanowisko : XPObject
	{
		public Stanowisko(Session session) : base(session)
		{ }


        string nazwa;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nazwa
        {
            get => nazwa;
            set => SetPropertyValue(nameof(Nazwa), ref nazwa, value);
        }
    }
}
