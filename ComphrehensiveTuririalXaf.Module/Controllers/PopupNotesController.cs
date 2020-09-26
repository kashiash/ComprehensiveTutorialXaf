using ComprehensiveTutorialXaf.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Controllers
{
  public  class PopupNotesController :ObjectViewController<DetailView,DemoTask>
    {
        PopupWindowShowAction ShowNotesAction;
        public PopupNotesController()
        {
            ShowNotesAction = new PopupWindowShowAction(this, $"{GetType().FullName}{nameof(ShowNotesAction)}", PredefinedCategory.Edit)
            {
                Caption = "Pokaż notatki",

            };
            ShowNotesAction.CustomizePopupWindowParams += ShowNotesAction_CustomizePopupWindowParams;
            ShowNotesAction.Execute += ShowNotesAction_Execute;
        }

        private void ShowNotesAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            string noteListViewId = Application.FindLookupListViewId(typeof(Note));
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Note), noteListViewId);
            e.View = Application.CreateListView(noteListViewId, collectionSource, true);
        }

        private void ShowNotesAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            DemoTask task = (DemoTask)View.CurrentObject;
            foreach (Note note in e.PopupWindowViewSelectedObjects)
            {
                if (!string.IsNullOrEmpty(task.Description))
                {
                    task.Description += Environment.NewLine;
                }
                task.Description += note.Text;
            }
            if (((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                View.ObjectSpace.CommitChanges();
            }
        }
    }
}
