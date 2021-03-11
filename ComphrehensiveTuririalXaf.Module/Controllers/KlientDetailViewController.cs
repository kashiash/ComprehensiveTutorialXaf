using Demo1.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Controllers
{
   public class KlientDetailViewController :ObjectViewController<DetailView,Klient>
    {
        PopupWindowShowAction showCustomers;

        public KlientDetailViewController()
        {
            TargetViewNesting = Nesting.Nested;

            showCustomers = new PopupWindowShowAction(this, $"{GetType().Name}.{nameof(showCustomers)}", DevExpress.Persistent.Base.PredefinedCategory.OpenObject)
            {
                Caption = "Wybierz klienta",
                ImageName = "BO_Skull",

            };
            showCustomers.Execute += ShowCustomers_Execute;
            showCustomers.CustomizePopupWindowParams += ShowCustomers_CustomizePopupWindowParams;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
      

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
            var parent = ObjectSpace.GetObject(((NestedFrame)Frame).ViewItem.CurrentObject as Faktura);
            foreach (var klient in e.PopupWindowViewSelectedObjects)
            {
                var pKlient = ObjectSpace.GetObject((Klient)klient);
                parent.ZmienKlienta(pKlient);
                View.Refresh();
                break;
            }

        }


        void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {

            var parent = ObjectSpace.GetObject(((NestedFrame)Frame).ViewItem.CurrentObject as Faktura);

            if (View.CurrentObject == e.Object &&
                   e.PropertyName == nameof(Klient.Nazwa) &&
                   ObjectSpace.IsModified &&
                   e.OldValue != e.NewValue)
            {
                Klient changedContact = (Klient)e.Object;
                var newCustomer = ObjectSpace.GetObjectsQuery<Klient>().Where(k => k.Nazwa == (string)e.NewValue).FirstOrDefault();
                if (parent != null && newCustomer != null)
                {
                    
                    parent.ZmienKlienta(newCustomer);
                    View.Refresh();
                    changedContact.Nazwa = (string)e.OldValue;
                }
             

            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
        }
    }
}
