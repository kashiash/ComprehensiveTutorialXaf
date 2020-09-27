using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace Common.BO.Adm
{
    [NavigationItem("Administracyjne")]
    [XafDefaultProperty(nameof(NazwaGminy))]
    public class Gmina : XPObject
    {
        public Gmina(Session session) : base(session)
        { }


        Wojewodztwo wojewodztwo;
        Powiat powiat;
        string nazwaGminy;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string NazwaGminy
        {
            get => nazwaGminy;
            set => SetPropertyValue(nameof(NazwaGminy), ref nazwaGminy, value);
        }


        [Association("Wojewodztwo-Gminy")]
        public Wojewodztwo Wojewodztwo
        {
            get => wojewodztwo;
            set => SetPropertyValue(nameof(Wojewodztwo), ref wojewodztwo, value);
        }

        [Association("Powiat-Gminy")]
        public Powiat Powiat
        {
            get => powiat;
            set => SetPropertyValue(nameof(Powiat), ref powiat, value);
        }

        [Association("Gmina-KodyPocztowe"), DevExpress.Xpo.Aggregated]
        public XPCollection<KodPocztowy> KodyPocztowe
        {
            get
            {
                return GetCollection<KodPocztowy>(nameof(KodyPocztowe));
            }
        }
    }
}
