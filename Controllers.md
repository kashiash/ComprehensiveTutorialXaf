*Kontrolery*

https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController
https://blog.delegate.at/2018/03/10/xaf-best-practices-2017-03.html

### Standardowa wersja kontrolera

```csharp
public class ClearContactsTaskControllerV1 : ViewController
{
    SimpleAction ClearTasksAction;
    public ClearContactsTaskControllerV1()
    {
        TargetObjectType = typeof(Contact); //Wskazujemy, ze dotyczy tylko obiektów klasy Contact
        TargetViewType = ViewType.DetailView; // Dotyczy tylko widoków typu DetailView
    }
    protected override void OnActivated()
        {
            base.OnActivated();
        }
    protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
   // ...
}
```
dokładnie ten sam efekt osiągniemy definiując kontroler w następujący sposób:
```csharp
public class ClearContactsTaskController : ViewController<DetailView>
{
    SimpleAction ClearTasksAction;
    public ClearContactsTaskController()
    {
        TargetObjectType = typeof(Contact);
        //TargetViewType = ViewType.DetailView; to juz jest niepotrzebne
}
...
```

albo tak:

```csharp
public class ClearContactsTaskControllerV2 : ObjectViewController<DetailView,Contact>
{
    SimpleAction ClearTasksAction;
    public ClearContactsTaskControllerV2()
    {
        //TargetObjectType = typeof(Contact); to juz nie jest potrzebne
        //TargetViewType = ViewType.DetailView;
    }
   // ...
}
```

## Kontroler pozwalający na dostęp do ustawień Grid'a na liście danych

[Na podstawie artykułu na stronie devexpress](https://docs.devexpress.com/eXpressAppFramework/113165/getting-started/comprehensive-tutorial/extend-functionality/access-grid-control-properties)

```csharp
public  class WinListViewController: ViewController<ListView>
  {
      GridListEditor gridListEditor = null;
      public WinListViewController()
      {
          ViewControlsCreated += WinAlternatingRowsController_ViewControlsCreated;
      }



      private void WinAlternatingRowsController_ViewControlsCreated(object sender, EventArgs e)
      {
          GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;
          if (listEditor != null)
          {
              GridView gridView = listEditor.GridView;
              gridView.OptionsView.EnableAppearanceOddRow = true;
              //  gridView.Appearance.OddRow.BackColor = Color.FromArgb(244, 244, 244);
              gridView.OptionsView.ShowFooter = false;
              // gridView.OptionsView.GroupFooterShowMode = GroupFooterShowMode.VisibleIfExpanded;
              gridView.OptionsPrint.ExpandAllGroups = false;
              //   właczamy filtry pod nagłowkami
              gridView.OptionsView.ShowAutoFilterRow = true;
              // właczamy scroll - ustaw false

              //checboxy do zaznaczania
              //gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;


              Object currentObject = this.View.Model.ModelClass;
              if (currentObject != null && currentObject.GetType() == typeof(DemoTask))
              {
                  // aby otrzymać multiline na gridzie
                  // właczamy zmiane rozmiru kolumn
                  gridView.OptionsView.RowAutoHeight = true;
                  gridView.OptionsView.ColumnAutoWidth = true;
              }

              else
              {
                  gridView.OptionsView.ColumnAutoWidth = false;
              }

          }
      }


  }
```

#### Simple Action

```csharp
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
             ConfirmationMessage = "Jesteś pewny tego czynu?",
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
```



#### Popup Window Action


##### Show Notes z tutoriala

```csharp
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
```


##### Wywołanie okna popup z wybranym obiektem z listy

```csharp
public class ShowDetailViewController : ViewController<ListView> {
    public ShowDetailViewController() {
            PopupWindowShowAction showPopupDetailViewAction = new PopupWindowShowAction(
                this, $"{GetType().FullName}.{nameof(showPopupDetailViewAction)}", PredefinedCategory.Edit)
            {
                ImageName = "BO_Skull",
                Caption = "Pokaż dane (popup)",
            };
            showPopupDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            showPopupDetailViewAction.TargetObjectsCriteria = "Not IsNewObject(This)";
            showPopupDetailViewAction.CustomizePopupWindowParams += showDetailViewAction_CustomizePopupWindowParams;
            showPopupDetailViewAction.Execute += showPopupDetailViewAction_Execute;
    }
    void showDetailViewAction_CustomizePopupWindowParams(
        object sender, CustomizePopupWindowParamsEventArgs e) {
        IObjectSpace newObjectSpace = Application.CreateObjectSpace(View.ObjectTypeInfo.Type);
        Object objectToShow = newObjectSpace.GetObject(View.CurrentObject);
        if (objectToShow != null) {
            DetailView createdView = Application.CreateDetailView(newObjectSpace, objectToShow);
            createdView.ViewEditMode = ViewEditMode.Edit;
            e.View = createdView;
        }
    }
    
            private void showPopupDetailViewAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            //będzie wywołane po zmaknięciu popup
            View.ObjectSpace.Refresh();
        }
}
```
##### Wywołanie okna za pomoca Simple Action (to samo co wyżej ale bez popup)

```csharp
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
```


##### Kontroler wywołujący kilka okien po sobie

<a href="https://docs.devexpress.com/eXpressAppFramework/112805/concepts/controllers-and-actions/dialog-controller" target="_blank">https://docs.devexpress.com/eXpressAppFramework/112805/concepts/controllers-and-actions/dialog-controller</a>



**Wersja testowa - do przećwiczenia**

<a href="https://supportcenter.devexpress.com/ticket/details/q382057/pop-up-consecutive-windows-win-and-web" target="_blank">https://supportcenter.devexpress.com/ticket/details/q382057/pop-up-consecutive-windows-win-and-web</a>

```csharp
[NonPersistent]
public class Parameters1
{
    [Size(-1)]
    public string Message1 { get; set; }
    public int PropertyNameInt { get; set; }
}

[NonPersistent]
public class Parameters2
{
    [Size(-1)]
    public string Message2 { get; set; }
    public DateTime PropertyNameDate { get; set; }
}

[NonPersistent]
public class Parameters3
{
    [Size(-1)]
    public string Message3 { get; set; }
    public decimal PropertyNameDecimal { get; set; }
}

[NonPersistent]
public class Parameters4
{
    [Size(-1)]
    public string Message4 { get; set; }
    public string PropertyNameString { get; set; }
}
public class ViewController1 : ViewController
{
    public ViewController1()
    {
        PopupWindowShowAction a1 = new PopupWindowShowAction(this, "Action1", DevExpress.Persistent.Base.PredefinedCategory.Unspecified);
        a1.CustomizePopupWindowParams += new CustomizePopupWindowParamsEventHandler(a1_CustomizePopupWindowParams);
        a1.Execute += new PopupWindowShowActionExecuteEventHandler(a1_Execute);
    }
    void a1_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        IObjectSpace os = ObjectSpaceInMemory.CreateNew();
        DetailView dv = Application.CreateDetailView(os, new Parameters1() { Message1 = "Message 1" });
        e.View = dv;
    }
    void a1_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        IObjectSpace os = ObjectSpaceInMemory.CreateNew();
        DetailView dv = Application.CreateDetailView(os, new Parameters2() { Message2 = "Message 2" });
        e.ShowViewParameters.CreatedView = dv;
        e.ShowViewParameters.Context = TemplateContext.PopupWindow;
        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;

        DialogController dc = Application.CreateController<DialogController>();
        e.ShowViewParameters.Controllers.Add(dc);
        dc.AcceptAction.Executed += new EventHandler<ActionBaseEventArgs>(a2_Execute);
    }
    void a2_Execute(object sender, ActionBaseEventArgs e)
    {
        IObjectSpace os = ObjectSpaceInMemory.CreateNew();
        DetailView dv = Application.CreateDetailView(os, new Parameters3() { Message3 = "Message 3" });
        e.ShowViewParameters.CreatedView = dv;
        e.ShowViewParameters.Context = TemplateContext.PopupWindow;
        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;

        DialogController dc = Application.CreateController<DialogController>();
        e.ShowViewParameters.Controllers.Add(dc);
        dc.AcceptAction.Executed += new EventHandler<ActionBaseEventArgs>(AcceptAction_Executed);
    }
    void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
    {
        IObjectSpace os = ObjectSpaceInMemory.CreateNew();
        DetailView dv = Application.CreateDetailView(os, new Parameters4() { Message4 = "Message 3" });
        e.ShowViewParameters.CreatedView = dv;
        e.ShowViewParameters.Context = TemplateContext.PopupWindow;
        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
    }
}
```
##### Inna wersja :

```csharp
public partial class WizardController : ViewController
 {
    

     public WizardController()
     {
         PopupWindowShowAction TaskStatusAction = new PopupWindowShowAction(this, $"{GetType().FullName}.{nameof(TaskStatusAction)}", PredefinedCategory.Edit)
         {
             SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
             Caption = "Wizard",
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

             var   wizardObjectSpace = Application.CreateObjectSpace();
        
             var objectToShow = wizardObjectSpace.GetObject(View.CurrentObject);
             DetailView dv = Application.CreateDetailView(wizardObjectSpace, objectToShow);
             dv.Caption = $"Action Result {step}";
             dialogController.AcceptAction.Caption = "Done";
             dialogController.CancelAction.Active.SetItemValue("InactiveReason", false);
             e.PopupWindow.SetView(dv);
         }
     }
 }
```


##### Zdalna pomoc

Praktycznie w każdej aplikacji powinniśmy dostarczyć możliwość wywołania TemaViewera, aby użytkownik mógł udostępnić pulpit do obsługi klienta za pomocą TeamViewer'a:
Następujący kontroler załatwia problem:
Wystarczy do katalogu z programem dograć **mediqus_serwis.exe**
```csharp
public  class ZdalnaPomocController: ViewController
  {
      SimpleAction ZdalnaPomocAction;
      public ZdalnaPomocController()
      {
          ZdalnaPomocAction = new SimpleAction(this, $"{GetType()}.{nameof(ZdalnaPomocAction)}", PredefinedCategory.Tools)
          {

              Caption = "Zdalna pomoc",

          };
          ZdalnaPomocAction.Execute += ZdalnaPomocAction_Execute;
      }

      private void ZdalnaPomocAction_Execute(object sender, SimpleActionExecuteEventArgs e)
      {
          ProcessStartInfo startInfo = new ProcessStartInfo { UseShellExecute = false, FileName = "mediqus_serwis.exe" };

          using (Process.Start(startInfo))
          {
          }
      }
  }
```
W ten sam sposób możemy uruchamiać każdy program lub otwierać dokument.
