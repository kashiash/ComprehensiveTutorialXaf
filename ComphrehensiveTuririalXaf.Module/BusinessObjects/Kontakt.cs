using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
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
    [XafDefaultProperty(nameof(Nazwisko))]
    [ImageName("BO_Contact")]
    [Appearance("KontaktArchiwalny",Criteria = "Archiwalny = true",TargetItems = "*", FontColor  = "Gray")]
    public class Kontakt : XPObject
    {
        public Kontakt(Session session) : base(session)
        { }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public string FullName
        {
            get
            {
                return ObjectFormatter.Format("{Imie} {Nazwisko}", this, EmptyEntriesMode.RemoveDelimeterWhenEntryIsEmpty);
            }
        }


        string email;
        Klient klient;
        string telefon;
        string nazwisko;
        string imie;
        bool archiwalny;
        Stanowisko stanowisko;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Imie
        {
            get => imie;
            set => SetPropertyValue(nameof(Imie), ref imie, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nazwisko
        {
            get => nazwisko;
            set => SetPropertyValue(nameof(Nazwisko), ref nazwisko, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Email
        {
            get => email;
            set => SetPropertyValue(nameof(Email), ref email, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Telefon
        {
            get => telefon;
            set => SetPropertyValue(nameof(Telefon), ref telefon, value);
        }


        public Stanowisko Stanowisko
        {
            get => stanowisko;
            set => SetPropertyValue(nameof(Stanowisko), ref stanowisko, value);
        }

        public bool Archiwalny
        {
            get => archiwalny;
            set => SetPropertyValue(nameof(Archiwalny), ref archiwalny, value);
        }


        [Association("Klient-Kontakty")]
        public Klient Klient
        {
            get => klient;
            set => SetPropertyValue(nameof(Klient), ref klient, value);
        }

        [Action(Caption = "Ustaw jako aktywny", ConfirmationMessage = "Are you sure?", ImageName = "BO_Active", AutoCommit = true)]
        public void ActiveActionMethod()
        {
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            this.Archiwalny = false;
        }
        [Action(Caption = "Ustaw jako archiwalny", ConfirmationMessage = "Are you sure?", ImageName = "BO_Inactive", AutoCommit = true)]
        public void ArchiveActionMethod()
        {
            // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            this.Archiwalny = true;
        }
    }
}
