using Demo1.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Controllers
{
    [DevExpress.ExpressApp.DC.DomainComponent]
    public class InvoiceTemplate
    {

        public InvoiceTemplate(Session session)
        {
            _Products = new XPCollection<Produkt>(session);
        }

        public DateTime DataFaktury { get; set; }
        public DateTime DataPlatnosci { get; set; }
        private XPCollection<Produkt> _Products;
     //   [XafDisplayName("Lista produktów")]
        public XPCollection<Produkt> Products { get { return _Products; } }
    }

    public class KlientListViewController : ObjectViewController<ListView, Klient>
    {

        public KlientListViewController()
        {
            PopupWindowShowAction action = new PopupWindowShowAction(this, "Wystaw faktury", PredefinedCategory.RecordEdit)
            {ImageName = "BO_Skull" };
            action.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            action.CustomizePopupWindowParams += new CustomizePopupWindowParamsEventHandler(action_CustomizePopupWindowParams);
            action.Execute += new PopupWindowShowActionExecuteEventHandler(action_Execute);
        }

        void action_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            e.Context = TemplateContext.PopupWindow;
            e.View = Application.CreateDetailView(os, new InvoiceTemplate(((DevExpress.ExpressApp.Xpo.XPObjectSpace)os).Session));
            ((DetailView)e.View).ViewEditMode = ViewEditMode.Edit;
        }
        void action_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            InvoiceTemplate parameters = e.PopupWindow.View.CurrentObject as InvoiceTemplate;
            ListPropertyEditor listPropertyEditor = ((DetailView)e.PopupWindow.View).FindItem("Products") as ListPropertyEditor;
            IObjectSpace os = Application.CreateObjectSpace();
            foreach (Klient klient in e.SelectedObjects)
            {
                var faktura = os.CreateObject<Faktura>();
                faktura.DataFaktury = parameters.DataFaktury;
                faktura.Klient = os.GetObject(klient);

                foreach (Produkt prod in listPropertyEditor.ListView.SelectedObjects)
                {
                    var pozycja = os.CreateObject<PozycjaFaktury>();
                    pozycja.Produkt = os.GetObject<Produkt>(prod);
                    pozycja.Ilosc = 1;
                    faktura.PozycjeFaktury.Add(pozycja);
                }
                faktura.Save();
            }
            os.CommitChanges();
        }
    }
}
