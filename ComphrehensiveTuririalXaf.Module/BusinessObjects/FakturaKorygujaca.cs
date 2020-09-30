using Demo1.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.BusinessObjects
{
    public class FakturaKorygujaca : Faktura
    {
        public FakturaKorygujaca(Session session) : base(session)
        { }


        Faktura fakturaKorygowana;

        public Faktura FakturaKorygowana
        {
            get => fakturaKorygowana;
            set => SetPropertyValue(nameof(FakturaKorygowana), ref fakturaKorygowana, value);
        }


        IList<PozycjaFaktury> pozycjePrzedKorekta;
        IList<PozycjaFaktury> PozycjePrzedKorekta
        {
            get
            {
                if (pozycjePrzedKorekta == null)
                {
                    PrzygotujPozycjePrzedKorekta();
                }
                return pozycjePrzedKorekta;
            }
        }


        

        private void PrzygotujPozycjePrzedKorekta()
        {
            pozycjePrzedKorekta = new List<PozycjaFaktury>();
        }
    }
}
