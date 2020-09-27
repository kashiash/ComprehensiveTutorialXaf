using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace Common.BO.Adm
{
    [NavigationItem("Administracyjne")]
    [XafDefaultProperty(nameof(Poczta))]
    [ModelDefault("","")]
    public class KodPocztowy : XPObject
    {
        public KodPocztowy(Session session) : base(session)
        { }


        string poczta;
        string kodUpr;
        Wojewodztwo wojewodztwo;

        Powiat powiat;
        Gmina gmina;
        string numery;
        string ulica;
        string kod;
        string miejscowosc;



        [Size(10)]
        public string Kod
        {
            get => kod;
            set => SetPropertyValue(nameof(Kod), ref kod, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Poczta
        {
            get => poczta;
            set => SetPropertyValue(nameof(Poczta), ref poczta, value);
        }

        [Size(10)]
        public string KodUpr
        {
            get => kodUpr;
            set => SetPropertyValue(nameof(KodUpr), ref kodUpr, value);
        }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Miejscowosc
        {
            get => miejscowosc;
            set => SetPropertyValue(nameof(Miejscowosc), ref miejscowosc, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Ulica
        {
            get => ulica;
            set => SetPropertyValue(nameof(Ulica), ref ulica, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Numery
        {
            get => numery;
            set => SetPropertyValue(nameof(Numery), ref numery, value);
        }


        [Association("Gmina-KodyPocztowe")]
        public Gmina Gmina
        {
            get => gmina;
            set => SetPropertyValue(nameof(Gmina), ref gmina, value);
        }

        [Association("Powiat-KodyPocztowe")]
        public Powiat Powiat
        {
            get => powiat;
            set => SetPropertyValue(nameof(Powiat), ref powiat, value);
        }



        [Association("Wojewodztwo-KodyPocztowe")]
        public Wojewodztwo Wojewodztwo
        {
            get => wojewodztwo;
            set => SetPropertyValue(nameof(Wojewodztwo), ref wojewodztwo, value);
        }
    }
}
