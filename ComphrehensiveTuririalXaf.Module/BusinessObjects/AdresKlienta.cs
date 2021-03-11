using Demo1.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.BusinessObjects
{
    public class AdresKlienta : Adres
    {
        public AdresKlienta(Session session) : base(session)
        { }


        Klient klient;

        [Association("Klient-AdresyKlienta")]
        public Klient Klient
        {
            get => klient;
            set => SetPropertyValue(nameof(Klient), ref klient, value);
        }
    }
}
