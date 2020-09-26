using ComphrehensiveTuririalXaf.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComphrehensiveTuririalXaf.Module.Controllers
{
    public class ClearContactsTaskController : ViewController<DetailView>
    {

        SimpleAction ClearTasksAction;

        public ClearContactsTaskController()
        {
            TargetObjectType = typeof(Contact);


            ClearTasksAction = new SimpleAction(this, $"{GetType().FullName}.{nameof(ClearTasksAction)} ", PredefinedCategory.Edit)
            {
                Caption = "Clear tasks",
                ImageName = "Action_Clear",
                PaintStyle = ActionItemPaintStyle.Image,
                ToolTip = "Clear tasks",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
            };
            ClearTasksAction.Execute += ClearTasks;
        }
            private void ClearTasks(object sender, SimpleActionExecuteEventArgs e)
            {
            var currentObject = View.CurrentObject as Contact;
        
                while (currentObject.Tasks.Count > 0)
                {
                    currentObject.Tasks.Remove(currentObject.Tasks[0]);
                }
            ObjectSpace.SetModified(currentObject);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

        }
        protected override void OnDeactivated()
        {

            base.OnDeactivated();
        }



    }
}
