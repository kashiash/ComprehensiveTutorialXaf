using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Controllers
{
    public class ShowDetailViewController : ViewController<ListView>
    {
        public ShowDetailViewController()
        {


            SimpleAction showSimpleDetailView = new SimpleAction(this, $"{GetType().FullName}.{nameof(showSimpleDetailView)}"
                , PredefinedCategory.Edit)
            {
                ImageName = "Pacifier",
                Caption = "Pokaż dane (simple)",

            };
            showSimpleDetailView.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            showSimpleDetailView.TargetObjectsCriteria = "Not IsNewObject(This)";
            showSimpleDetailView.Execute += showSimpleDetailView_Execute;

        }



        private void showSimpleDetailView_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace newObjectSpace = Application.CreateObjectSpace(View.ObjectTypeInfo.Type);
            Object objectToShow = newObjectSpace.GetObject(View.CurrentObject);
            if (objectToShow != null)
            {
                string detailId = Application.FindDetailViewId(objectToShow.GetType());
                DetailView detailView = Application.CreateDetailView(newObjectSpace, detailId, true, objectToShow);
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
            View.ObjectSpace.Refresh();
        }


    }
}
