using Demo1.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Controllers
{
   public class FakturaDetailViewController : ObjectViewController<DetailView,Faktura>
    {

        PopupWindowShowAction showCustomersAction;
        PopupWindowShowAction findCustomerAction;
        public FakturaDetailViewController()
        {
            showCustomersAction = new PopupWindowShowAction(this, $"{GetType().Name}.{nameof(showCustomersAction)}", DevExpress.Persistent.Base.PredefinedCategory.OpenObject)
            {
                Caption = "Wybierz klienta",
                ImageName = "BO_Skull",

            };
            showCustomersAction.Execute += ShowCustomers_Execute;
            showCustomersAction.CustomizePopupWindowParams += ShowCustomers_CustomizePopupWindowParams;

            findCustomerAction = new PopupWindowShowAction(this, $"{GetType().Name}.{nameof(findCustomerAction)}", DevExpress.Persistent.Base.PredefinedCategory.OpenObject)
            {
                Caption = "Wybierz klienta",
                ImageName = "BO_Skull",

            };
            findCustomerAction.Execute += ShowCustomers_Execute;
            findCustomerAction.CustomizePopupWindowParams += ShowCustomers_CustomizePopupWindowParams;

        }

        private void ShowCustomers_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            string noteListViewId = Application.FindListViewId(typeof(Klient));
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Klient), noteListViewId);
            e.View = Application.CreateListView(noteListViewId, collectionSource, true);
        }

        private void ShowCustomers_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var faktura = (Faktura)View.CurrentObject;
            foreach (var klient in e.PopupWindowViewSelectedObjects)
            {
                var pKlient = ObjectSpace.GetObject((Klient)klient);
                faktura.ZmienKlienta(pKlient);
                break;
            }

        }
    }
}
