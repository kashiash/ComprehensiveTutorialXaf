using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace Common.BO.Adm
{
    [NavigationItem("Administracyjne")]
    [XafDefaultProperty(nameof(NazwaPowiatu))]
    public class Powiat : XPObject
    {
        public Powiat(Session session) : base(session)
        { }


        Wojewodztwo wojewodztwo;
        string nazwaPowiatu;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string NazwaPowiatu
        {
            get => nazwaPowiatu;
            set => SetPropertyValue(nameof(NazwaPowiatu), ref nazwaPowiatu, value);
        }


        [Association("Wojewodztwo-Powiaty")]
        public Wojewodztwo Wojewodztwo
        {
            get => wojewodztwo;
            set => SetPropertyValue(nameof(Wojewodztwo), ref wojewodztwo, value);
        }

        [Association("Powiat-Gminy"), DevExpress.Xpo.Aggregated]
        public XPCollection<Gmina> Gminy
        {
            get
            {
                return GetCollection<Gmina>(nameof(Gminy));
            }
        }
        [Association("Powiat-KodyPocztowe"), DevExpress.Xpo.Aggregated]
        public XPCollection<KodPocztowy> KodyPocztowe
        {
            get
            {
                return GetCollection<KodPocztowy>(nameof(KodyPocztowe));
            }
        }
    }
}
