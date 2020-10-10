using Demo1.Module.BusinessObjects;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.BusinessObjects
{
    [DomainComponent]
    public class FakturaDoRozliczeniaDC
    {
        public FakturaDoRozliczeniaDC()
        {

        }
        public Faktura Faktura { get; set; }
        public decimal KwotaDoRozliczenia { get; set; }
    }
}
