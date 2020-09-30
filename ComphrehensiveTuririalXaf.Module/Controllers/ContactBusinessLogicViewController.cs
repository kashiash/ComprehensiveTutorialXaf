using ComprehensiveTutorialXaf.Module.BusinessObjects;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Controllers
{
    public class ContactBusinessLogicViewController : ObjectViewController<ObjectView, WplataDoRozliczeniaDC>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
        }
        void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (View.CurrentObject == e.Object &&
                   e.PropertyName == "Naleznosci" &&
                   ObjectSpace.IsModified &&
                   e.OldValue != e.NewValue)
            {
                WplataDoRozliczeniaDC changedRecord = (WplataDoRozliczeniaDC)e.Object;
                if (changedRecord != null)
                {
                    var a = changedRecord.SumaRozliczen;
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
