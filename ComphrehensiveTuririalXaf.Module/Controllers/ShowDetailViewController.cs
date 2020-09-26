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
            PopupWindowShowAction showDetailViewAction = new PopupWindowShowAction(
                this, "ShowDetailView", PredefinedCategory.Edit)
            {
                ImageName = "BO_Skull",

            };
            showDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            showDetailViewAction.TargetObjectsCriteria = "Not IsNewObject(This)";
            showDetailViewAction.CustomizePopupWindowParams += showDetailViewAction_CustomizePopupWindowParams;
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
