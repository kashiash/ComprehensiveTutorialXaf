using ComprehensiveTutorialXaf.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ComprehensiveTutorialXaf.Module.Controllers
{
  public  class DemoTaskController : ObjectViewController<ListView,DemoTask>
    {
        SingleChoiceAction ChangeStatusAction;
        public DemoTaskController()
        {
            ChangeStatusAction = new SingleChoiceAction(this, $"{GetType().FullName}.{nameof(ChangeStatusAction)}", DevExpress.Persistent.Base.PredefinedCategory.Edit) { 
            Caption = "Zmień status",
            ImageName = "BO_Task",
            ItemType = SingleChoiceActionItemType.ItemIsOperation,
            ToolTip= "Zmienia status zadania",
            ConfirmationMessage = "Czy jesteś pełen władz umysłowych i chcesz to zrobić?",

            };
            SetActionItems(ChangeStatusAction,typeof(TaskStatus));
            ChangeStatusAction.Execute += ChangeStatusAction_Execute;
        }

        private void ChangeStatusAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace(typeof(DemoTask));
            foreach(var obj in  e.SelectedObjects)
                {
                var myObj = (DemoTask)os.GetObject(obj);
                myObj.Status = (TaskStatus)e.SelectedChoiceActionItem.Data;
            }

            os.CommitChanges();
            View.ObjectSpace.Refresh();
        }

        private void SetActionItems(SingleChoiceAction action, Type type)
        {
            foreach (var status in Enum.GetValues(type))
            {
                var enumDescription = new EnumDescriptor(type);
                var item = new ChoiceActionItem(enumDescription.GetCaption(status), status);
                item.ImageName = ImageLoader.Instance.GetEnumValueImageName(status);
                action.Items.Add(item);
            }
        }
    }
}
