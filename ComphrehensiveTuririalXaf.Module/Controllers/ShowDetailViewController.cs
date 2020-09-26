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
            PopupWindowShowAction showPopupDetailViewAction = new PopupWindowShowAction(
                this, $"{GetType().FullName}.{nameof(showPopupDetailViewAction)}", PredefinedCategory.Edit)
            {
                ImageName = "BO_Skull",

            };
            showPopupDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            showPopupDetailViewAction.TargetObjectsCriteria = "Not IsNewObject(This)";
            showPopupDetailViewAction.CustomizePopupWindowParams += showDetailViewAction_CustomizePopupWindowParams;


            SimpleAction showSimpleDetailView = new SimpleAction(this, $"{GetType().FullName}.{nameof(showSimpleDetailView)}"
                , PredefinedCategory.Edit)
            {
                ImageName = "Pacifier",

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
            }
        }

        void showDetailViewAction_CustomizePopupWindowParams(
            object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace newObjectSpace = Application.CreateObjectSpace(View.ObjectTypeInfo.Type);
            Object objectToShow = newObjectSpace.GetObject(View.CurrentObject);
            if (objectToShow != null)
            {
                DetailView createdView = Application.CreateDetailView(newObjectSpace, objectToShow);
                createdView.ViewEditMode = ViewEditMode.Edit;
                e.View = createdView;
            }
        }
    }
}
