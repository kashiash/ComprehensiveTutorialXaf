using Demo1.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.BusinessObjects
{


    public class PozycjaFakturyKorygujacej : PozycjaFaktury
    {
        public PozycjaFakturyKorygujacej(Session session) : base(session)
        { }


        PozycjaFaktury pozycjaKorygowana;

        public PozycjaFaktury PozycjaKorygowana
        {
            get => pozycjaKorygowana;
            set => SetPropertyValue(nameof(PozycjaKorygowana), ref pozycjaKorygowana, value);
        }

    }
}
