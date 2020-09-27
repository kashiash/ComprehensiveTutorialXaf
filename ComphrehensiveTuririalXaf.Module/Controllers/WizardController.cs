using ComprehensiveTutorialXaf.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Controllers
{
    public partial class WizardController : ViewController
    {
       

        public WizardController()
        {
            PopupWindowShowAction TaskStatusAction = new PopupWindowShowAction(this, $"{GetType().FullName}.{nameof(TaskStatusAction)}", PredefinedCategory.Edit)
            {
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
                Caption = "Wizard",
                ImageName= "witch",
            };

            TargetObjectType = typeof(DemoTask);
            TaskStatusAction.CustomizePopupWindowParams += TaskStatusAction_CustomizePopupWindowParams;
            TaskStatusAction.Execute += TaskStatusAction_Execute;

        }


        private void TaskStatusAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            var objectToShow = os.GetObject(View.CurrentObject);
            DetailView dv = Application.CreateDetailView(os, objectToShow, false);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            e.View = dv;
            step = 0;
        }

        int step;
   
        private void TaskStatusAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            DialogController dialogController = e.PopupWindow.GetController<DialogController>();

            ProcessStep(e, dialogController, step);
            step++;
        }

        private void ProcessStep(PopupWindowShowActionExecuteEventArgs e, DialogController dialogController, int step)
        {

            if (step == 10)
            {
                e.CanCloseWindow = true;
                //  TaskStatusAction.Enabled["By Criteria"] = savedEnabledItem;
                dialogController.CancelAction.Active.RemoveItem("InactiveReason");
            }
            else
            {

                (View.CurrentObject as DemoTask).Status = DevExpress.Persistent.Base.General.TaskStatus.InProgress;
                View.ObjectSpace.CommitChanges();

                e.CanCloseWindow = false;

                var   wizardObjectSpace  = Application.CreateObjectSpace();
           
                var objectToShow = wizardObjectSpace.GetObject(View.CurrentObject);
                DetailView dv = Application.CreateDetailView(wizardObjectSpace, objectToShow);
                dv.Caption = $"Action Result {step}";
                dialogController.AcceptAction.Caption = $"Dalej ({step})";
                dialogController.CancelAction.Active.SetItemValue("InactiveReason", false);
                e.PopupWindow.SetView(dv);
            }
        }
    }
}
