using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Adres : XPObject
    {
        public Adres(Session session) : base(session)
        { }

        string nrMieszkania;
        string nrDomu;
        string miejscowosc;
        string kodPocztowy;
        string ulica;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Ulica
        {
            get => ulica;
            set => SetPropertyValue(nameof(Ulica), ref ulica, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string NrDomu
        {
            get => nrDomu;
            set => SetPropertyValue(nameof(NrDomu), ref nrDomu, value);
        }
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string NrMieszkania
        {
            get => nrMieszkania;
            set => SetPropertyValue(nameof(NrMieszkania), ref nrMieszkania, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string KodPocztowy
        {
            get => kodPocztowy;
            set => SetPropertyValue(nameof(KodPocztowy), ref kodPocztowy, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Miejscowosc
        {
            get => miejscowosc;
            set => SetPropertyValue(nameof(Miejscowosc), ref miejscowosc, value);
        }

        [Browsable(false)]
        [NonPersistent]
        public bool IsNewObject
        {
            get
            {
                return Session.IsNewObject(this);
            }
        }
    }
}
