using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo1.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class PozycjaFaktury : XPObject
    {
        public PozycjaFaktury(Session session) : base(session)
        { }


        Faktura faktura;
        decimal wartoscVAT;
        decimal wartoscBrutto;
        decimal wartoscNetto;
        decimal cena;
        decimal ilosc;
        Produkt produkt;


        [ImmediatePostData]
        public Produkt Produkt
        {
            get => produkt;
            set
            {
                bool modified = SetPropertyValue(nameof(Produkt), ref produkt, value);
                if (modified && !IsLoading && !IsSaving && Produkt != null)
                {
                    Cena = Produkt.Cena;
                    PrzeliczPozycje();

                }
            }
        }


        [Association]
        public Faktura Faktura
        {
            get => faktura;
            set
            {
                bool modified = SetPropertyValue(nameof(Faktura), ref faktura, value);
                if (!IsLoading && !IsSaving && Faktura != null && modified)
                {
                    Faktura.PrzeliczSumy(true);

                }
            }
        }
        [ImmediatePostData]
        public decimal Ilosc
        {
            get => ilosc;
            set
            {
                bool modified = SetPropertyValue(nameof(Ilosc), ref ilosc, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    PrzeliczPozycje();

                }
            }
        }

        private void PrzeliczPozycje()
        {
            WartoscNetto = Ilosc * Cena;
            if (Produkt != null && Produkt.StawkaVAT != null)
            {
                WartoscBrutto = WartoscNetto * (100 + Produkt.StawkaVAT.Stawka) / 100;
            }
            else
            {
                WartoscBrutto = WartoscNetto;

            }
            WartoscVAT = WartoscBrutto - WartoscNetto;

            if (Faktura != null)
            {
                Faktura.PrzeliczSumy(true);
            }
        }
        [ImmediatePostData]
        public decimal Cena
        {
            get => cena;
            set
            {

                bool modified = SetPropertyValue(nameof(Cena), ref cena, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    PrzeliczPozycje();

                }
            }
        }


        public decimal WartoscNetto
        {
            get => wartoscNetto;
            set => SetPropertyValue(nameof(WartoscNetto), ref wartoscNetto, value);
        }

        public decimal WartoscVAT
        {
            get => wartoscVAT;
            set => SetPropertyValue(nameof(WartoscVAT), ref wartoscVAT, value);
        }

        public decimal WartoscBrutto
        {
            get => wartoscBrutto;
            set => SetPropertyValue(nameof(WartoscBrutto), ref wartoscBrutto, value);
        }
    }
}
