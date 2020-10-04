using ComprehensiveTutorialXaf.Module.Factory;
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
   public class FakturyViewController : ObjectViewController<ListView,Faktura>
    {
        SimpleAction wystawKorekteAction;
        public FakturyViewController()
        {
            wystawKorekteAction = new SimpleAction(this, $"{GetType().FullName}.{nameof(wystawKorekteAction)}", DevExpress.Persistent.Base.PredefinedCategory.Edit)
            {
                Caption = "WystawKorekte",
            };

            wystawKorekteAction.Execute += WystawKorekteAction_Execute;  
        }

        private void WystawKorekteAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            var factory = new InvoiceFactory(os);
            var wybranaFaktura = os.GetObject(View.CurrentObject as Faktura);
          var korekta =  factory.UtworzKorekteCalkowita(wybranaFaktura);
            if (korekta != null)
            {
                string detailId = Application.FindDetailViewId(korekta.GetType());
                DetailView detailView = Application.CreateDetailView(os, detailId, true, korekta);
                e.ShowViewParameters.CreatedView = detailView;
                e.ShowViewParameters.Context = TemplateContext.View;
                e.ShowViewParameters.TargetWindow = TargetWindow.Default;
                // po zamknieciu okna zostanie wywołane zdarzenie
                detailView.Closed += DetailView_Closed;
            }

        }

        private void DetailView_Closed(object sender, EventArgs e)
        {
            //będzie wywołane po zamknięciu zwykłego okna
          //  View.ObjectSpace.Refresh();
        }
    }
}
